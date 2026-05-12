// Assets/Speedma/Scripts/SpeedmaSimManager.cs
// ============================================================
// Runtime counterpart of the Editor FMU Tester.
// Runs an FMU session in Play mode via the Speedma backend.
//
// - Starts session on Awake
// - Steps at a fixed interval (SimDt)
// - Exposes GetOutput(name) / SetInput(name, value)
// - FmuSceneLink + other scripts call these each frame
// ============================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Speedma
{
    public class SpeedmaSimManager : MonoBehaviour
    {
        [Header("Backend")]
        [SerializeField] private string backendUrl = "http://localhost:8000";
        [SerializeField] private string fmuName    = "CircuitoRC_Interativo";

        [Header("Simulation")]
        [SerializeField] private float simDt = 0.001f;   // keep <= tau/10

        // ── State ────────────────────────────────────────────────────
        public bool   IsSessionActive { get; private set; }
        public float  SimTime         { get; private set; }
        public string SessionId       { get; private set; }
        public string StatusMessage   { get; private set; } = "Idle";

        private Dictionary<string, float>  _outputs = new();
        private Dictionary<string, object> _inputs  = new();
        private bool _stepping = false;

        // ══════════════════════════════════════════════════════════════

        private void Awake()
        {
            StartCoroutine(StartSession());
        }

        private void OnDestroy()
        {
            if (!string.IsNullOrEmpty(SessionId))
                StartCoroutine(StopSession());
        }

        // ── Public API ───────────────────────────────────────────────

        public float GetOutput(string name)
            => _outputs.TryGetValue(name, out var v) ? v : 0f;

        public void SetInput(string name, float value)   => _inputs[name] = value;
        public void SetInput(string name, bool  value)   => _inputs[name] = value;
        public void SetInput(string name, int   value)   => _inputs[name] = value;

        // ── Session lifecycle ────────────────────────────────────────

        private IEnumerator StartSession()
        {
            StatusMessage = "Starting session...";
            string body = $"{{\"fmu_name\":\"{fmuName}\"}}";
            yield return PostJson($"{backendUrl}/sim/start", body, json =>
            {
                SessionId     = ParseString(json, "session_id");
                IsSessionActive = !string.IsNullOrEmpty(SessionId);
                StatusMessage = IsSessionActive ? "Session active" : "Failed to start";
            });

            if (IsSessionActive)
                StartCoroutine(StepLoop());
        }

        private IEnumerator StepLoop()
        {
            var wait = new WaitForSecondsRealtime(simDt);
            while (IsSessionActive)
            {
                if (!_stepping)
                {
                    _stepping = true;
                    yield return SendStep();
                    _stepping = false;
                }
                yield return wait;
            }
        }

        private IEnumerator SendStep()
        {
            var sb = new StringBuilder();
            sb.Append($"{{\"session_id\":\"{SessionId}\",");
            sb.Append($"\"dt\":{simDt.ToString(System.Globalization.CultureInfo.InvariantCulture)},");
            sb.Append("\"inputs\":{");
            bool first = true;
            foreach (var kv in _inputs)
            {
                if (!first) sb.Append(',');
                first = false;
                string val = kv.Value is bool b ? (b ? "true" : "false")
                           : Convert.ToString(kv.Value, System.Globalization.CultureInfo.InvariantCulture);
                sb.Append($"\"{kv.Key}\":{val}");
            }
            sb.Append("}}");

            yield return PostJson($"{backendUrl}/sim/step", sb.ToString(), json =>
            {
                SimTime = ParseFloat(json, "\"t\"");
                // parse all outputs from flat JSON
                ParseAllFloats(json, _outputs);
                StatusMessage = $"t = {SimTime:F3} s";
            });
        }

        private IEnumerator StopSession()
        {
            IsSessionActive = false;
            yield return PostJson($"{backendUrl}/sim/stop",
                $"{{\"session_id\":\"{SessionId}\"}}", _ => { });
        }

        // ── HTTP helpers ─────────────────────────────────────────────

        private IEnumerator PostJson(string url, string body, Action<string> onSuccess)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(body);
            using var req = new UnityWebRequest(url, "POST");
            req.uploadHandler   = new UploadHandlerRaw(bytes);
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");
            req.timeout = 5;
            yield return req.SendWebRequest();
            if (req.result == UnityWebRequest.Result.Success)
                onSuccess(req.downloadHandler.text);
            else
                StatusMessage = $"Error: {req.error}";
        }

        // ── JSON helpers ─────────────────────────────────────────────

        private static string ParseString(string json, string key)
        {
            string s = $"\"{key}\"";
            int i = json.IndexOf(s, StringComparison.Ordinal);
            if (i < 0) return string.Empty;
            i += s.Length;
            while (i < json.Length && (json[i] == ' ' || json[i] == ':')) i++;
            if (i >= json.Length || json[i] != '"') return string.Empty;
            i++; int end = json.IndexOf('"', i);
            return end < 0 ? string.Empty : json.Substring(i, end - i);
        }

        private static float ParseFloat(string json, string key)
        {
            int i = json.IndexOf(key, StringComparison.Ordinal);
            if (i < 0) return 0f;
            i += key.Length;
            while (i < json.Length && (json[i] == ' ' || json[i] == ':')) i++;
            int end = i;
            while (end < json.Length && (char.IsDigit(json[end]) || json[end] == '.' ||
                   json[end] == '-' || json[end] == 'e' || json[end] == 'E' || json[end] == '+')) end++;
            return float.TryParse(json.Substring(i, end - i),
                System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out float v) ? v : 0f;
        }

        // Extracts every numeric value in the output JSON into the dictionary
        private static void ParseAllFloats(string json, Dictionary<string, float> dict)
        {
            // Looks for "outputs":{...} block first, falls back to flat scan
            int start = json.IndexOf("\"outputs\"", StringComparison.Ordinal);
            string block = start >= 0 ? json.Substring(start) : json;

            int pos = 0;
            while (pos < block.Length)
            {
                int q1 = block.IndexOf('"', pos);       if (q1 < 0) break;
                int q2 = block.IndexOf('"', q1 + 1);   if (q2 < 0) break;
                string key = block.Substring(q1 + 1, q2 - q1 - 1);
                pos = q2 + 1;
                while (pos < block.Length && (block[pos] == ' ' || block[pos] == ':')) pos++;
                if (pos >= block.Length) break;
                if (block[pos] == '"' || block[pos] == '{' || block[pos] == '[') continue;
                int end = pos;
                while (end < block.Length && (char.IsDigit(block[end]) || block[end] == '.' ||
                       block[end] == '-' || block[end] == 'e' || block[end] == 'E' || block[end] == '+')) end++;
                if (end > pos && float.TryParse(block.Substring(pos, end - pos),
                    System.Globalization.NumberStyles.Float,
                    System.Globalization.CultureInfo.InvariantCulture, out float v))
                    dict[key] = v;
                pos = end;
            }
        }
    }
}
