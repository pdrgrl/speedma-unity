using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// WebGL-safe ISimulationBackend implementation.
/// Drives the FMU that lives on the Python backend via HTTP:
///
///   POST /sim/start   → get session_id
///   POST /sim/step    → send inputs, receive outputs
///   POST /sim/reset   → rewind to t = 0
///   POST /sim/stop    → free server resources on quit
///
/// Input writes are buffered locally and flushed on the next Step call.
/// Output reads return the values received from the PREVIOUS step
/// (one-frame latency), which is fine for a 50 Hz simulation loop.
/// </summary>
public class RemoteFmuSimulation : MonoBehaviour, ISimulationBackend
{
    // ── Inspector ──────────────────────────────────────────────────
    [Tooltip("Base URL of the Python backend (no trailing slash).")]
    public string backendUrl = "http://127.0.0.1:8000";

    [Tooltip("Override the default FMU path on the server. Leave blank to use server default.")]
    public string fmuPathOverride = "";

    // ── ISimulationBackend ──────────────────────────────────────────
    public bool IsReady => _ready;

    public void SetBoolean(string name, bool value)  => _pendingInputs[name] = value;
    public void SetReal(string name, double value)   => _pendingInputs[name] = value;
    public void SetInteger(string name, int value)   => _pendingInputs[name] = value;

    public float GetReal(string name)    => _outputs.TryGetValue(name, out var v) ? Convert.ToSingle(v) : 0f;
    public bool  GetBoolean(string name) => _outputs.TryGetValue(name, out var v) && Convert.ToBoolean(v);
    public int   GetInteger(string name) => _outputs.TryGetValue(name, out var v) ? Convert.ToInt32(v) : 0;

    public void Step(float dt)
    {
        if (!_ready || _stepping) return;
        StartCoroutine(DoStep(dt));
    }

    public void Reset()
    {
        if (!_ready) return;
        StartCoroutine(DoReset());
    }

    // ── Private state ────────────────────────────────────────────────
    private string  _sessionId;
    private bool    _ready    = false;
    private bool    _stepping = false;
    private readonly Dictionary<string, object> _pendingInputs = new();
    private readonly Dictionary<string, object> _outputs       = new();

    // ── Unity lifecycle ──────────────────────────────────────────────
    void Start() => StartCoroutine(StartSession());

    void OnApplicationQuit()
    {
        if (!string.IsNullOrEmpty(_sessionId))
            StartCoroutine(StopSession());
    }

    // ── Coroutines ─────────────────────────────────────────────────
    IEnumerator StartSession()
    {
        var body    = string.IsNullOrEmpty(fmuPathOverride)
            ? "{}"
            : $"{{\"fmu_path\":\"{fmuPathOverride}\"}}";
        var bytes   = Encoding.UTF8.GetBytes(body);

        using var req = new UnityWebRequest(backendUrl + "/sim/start", "POST");
        req.uploadHandler   = new UploadHandlerRaw(bytes);
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");
        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            var resp     = JsonUtility.FromJson<StartResponse>(req.downloadHandler.text);
            _sessionId   = resp.session_id;
            _ready       = true;
            Debug.Log($"[RemoteFmu] Session started: {_sessionId}");
        }
        else
        {
            Debug.LogError($"[RemoteFmu] Could not start session: {req.error}");
        }
    }

    IEnumerator DoStep(float dt)
    {
        _stepping = true;

        // Snapshot + clear pending inputs atomically
        var snapshot = new Dictionary<string, object>(_pendingInputs);
        _pendingInputs.Clear();

        // Build JSON manually (avoids System.Text.Json in WebGL)
        var inputsJson = BuildInputsJson(snapshot);
        var body       = $"{{\"session_id\":\"{_sessionId}\",\"dt\":{dt.ToString("F4", System.Globalization.CultureInfo.InvariantCulture)},\"inputs\":{inputsJson}}}";
        var bytes      = Encoding.UTF8.GetBytes(body);

        using var req = new UnityWebRequest(backendUrl + "/sim/step", "POST");
        req.uploadHandler   = new UploadHandlerRaw(bytes);
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");
        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            // Parse flat key:value outputs from JSON
            ParseOutputs(req.downloadHandler.text);
        }
        else
        {
            Debug.LogWarning($"[RemoteFmu] Step error: {req.error}");
        }

        _stepping = false;
    }

    IEnumerator DoReset()
    {
        var body  = $"{{\"session_id\":\"{_sessionId}\"}}";
        var bytes = Encoding.UTF8.GetBytes(body);

        using var req = new UnityWebRequest(backendUrl + "/sim/reset", "POST");
        req.uploadHandler   = new UploadHandlerRaw(bytes);
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");
        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
            Debug.Log("[RemoteFmu] Session reset.");
        else
            Debug.LogWarning($"[RemoteFmu] Reset error: {req.error}");
    }

    IEnumerator StopSession()
    {
        var body  = $"{{\"session_id\":\"{_sessionId}\"}}";
        var bytes = Encoding.UTF8.GetBytes(body);

        using var req = new UnityWebRequest(backendUrl + "/sim/stop", "POST");
        req.uploadHandler   = new UploadHandlerRaw(bytes);
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");
        yield return req.SendWebRequest();
    }

    // ── JSON helpers (no external deps) ───────────────────────────────

    static string BuildInputsJson(Dictionary<string, object> dict)
    {
        if (dict.Count == 0) return "{}";
        var sb = new StringBuilder("{");
        bool first = true;
        foreach (var kv in dict)
        {
            if (!first) sb.Append(',');
            first = false;
            sb.Append($"\"{kv.Key}\":");
            switch (kv.Value)
            {
                case bool b:   sb.Append(b ? "true" : "false"); break;
                case int  i:   sb.Append(i); break;
                case float f:  sb.Append(f.ToString("F6", System.Globalization.CultureInfo.InvariantCulture)); break;
                case double d: sb.Append(d.ToString("F6", System.Globalization.CultureInfo.InvariantCulture)); break;
                default:       sb.Append($"\"{kv.Value}\""); break;
            }
        }
        sb.Append('}');
        return sb.ToString();
    }

    void ParseOutputs(string json)
    {
        // Extract the "outputs":{...} block and parse key:value pairs.
        // Using lightweight manual parse to avoid Newtonsoft/System.Text.Json.
        int start = json.IndexOf("\"outputs\":", StringComparison.Ordinal);
        if (start < 0) return;
        start += 10; // skip "outputs":
        int depth = 0, objStart = -1, objEnd = -1;
        for (int i = start; i < json.Length; i++)
        {
            if (json[i] == '{') { if (depth++ == 0) objStart = i; }
            else if (json[i] == '}') { if (--depth == 0) { objEnd = i; break; } }
        }
        if (objStart < 0 || objEnd < 0) return;
        string inner = json.Substring(objStart + 1, objEnd - objStart - 1).Trim();
        if (string.IsNullOrEmpty(inner)) return;

        _outputs.Clear();
        foreach (var pair in inner.Split(','))
        {
            var kv = pair.Split(new[] { ':' }, 2);
            if (kv.Length != 2) continue;
            string key = kv[0].Trim().Trim('"');
            string val = kv[1].Trim();
            if (val == "true")       _outputs[key] = true;
            else if (val == "false") _outputs[key] = false;
            else if (float.TryParse(val, System.Globalization.NumberStyles.Float,
                                    System.Globalization.CultureInfo.InvariantCulture, out float f))
                _outputs[key] = f;
            else
                _outputs[key] = val.Trim('"');
        }
    }

    // ── Response DTOs ────────────────────────────────────────────────
    [Serializable] class StartResponse { public string session_id; }
}
