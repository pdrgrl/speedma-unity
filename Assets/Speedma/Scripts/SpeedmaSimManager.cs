// Assets/Speedma/Scripts/SpeedmaSimManager.cs
// ============================================================
// Runtime counterpart of the Editor FMU Tester.
// Mirrors FmuTester exactly: send step → wait response → repeat.
// Inputs persist between steps (never cleared).
// ============================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Speedma
{
    public class SpeedmaSimManager : MonoBehaviour
    {
        [Header("Backend")]
        [SerializeField]
        private string backendUrl = "http://localhost:8000";

        [SerializeField]
        private string fmuName = "CircuitoRC_Interativo";

        [Header("Simulation")]
        [SerializeField]
        private float simDt = 0.001f;

        // ── State ────────────────────────────────────────────────────
        public bool IsSessionActive { get; private set; }
        public float SimTime { get; private set; }
        public string SessionId { get; private set; }
        public string StatusMessage { get; private set; } = "Idle";

        // Outputs are read by FmuSceneLink every Unity frame
        private readonly Dictionary<string, float> _outputs = new();

        // Inputs persist — never cleared, overwritten by SetInput()
        private readonly Dictionary<string, object> _inputs = new();

        // ══════════════════════════════════════════════════════════════

        private void Awake() => StartCoroutine(StartSession());

        private void OnDestroy()
        {
            if (!string.IsNullOrEmpty(SessionId))
                _ = StopSessionAsync(backendUrl, SessionId);
        }

        // ── Public API ───────────────────────────────────────────────
        public float GetOutput(string name) => _outputs.TryGetValue(name, out var v) ? v : 0f;

        public void SetInput(string name, float value) => _inputs[name] = value;

        public void SetInput(string name, bool value) => _inputs[name] = value;

        public void SetInput(string name, int value) => _inputs[name] = value;

        // ── Session lifecycle ────────────────────────────────────────

        private IEnumerator StartSession()
        {
            StatusMessage = "Starting session...";
            yield return PostJson(
                $"{backendUrl}/sim/start",
                $"{{\"fmu_name\":\"{fmuName}\"}}",
                json =>
                {
                    SessionId = ParseString(json, "session_id");
                    IsSessionActive = !string.IsNullOrEmpty(SessionId);
                    StatusMessage = IsSessionActive ? "Session active" : "Failed to start";
                }
            );

            if (IsSessionActive)
                StartCoroutine(StepLoop());
        }

        // Mirrors FmuTester: one step at a time, wait for HTTP response.
        // yield return null gives Unity one frame between steps so the
        // scene can read outputs and write new inputs before the next send.
        private IEnumerator StepLoop()
        {
            while (IsSessionActive)
            {
                yield return SendStep(); // wait for full HTTP round-trip
                yield return null; // one frame pause → FmuSceneLink reads, FmuDebugController writes
            }
        }

        private IEnumerator SendStep()
        {
            var sb = new StringBuilder();
            sb.Append($"{{\"session_id\":\"{SessionId}\",");
            sb.Append(
                $"\"dt\":{simDt.ToString(System.Globalization.CultureInfo.InvariantCulture)},"
            );
            sb.Append("\"inputs\":{");
            bool first = true;
            foreach (var kv in _inputs)
            {
                if (!first)
                    sb.Append(',');
                first = false;
                string val = kv.Value is bool b
                    ? (b ? "true" : "false")
                    : Convert.ToString(kv.Value, System.Globalization.CultureInfo.InvariantCulture);
                sb.Append($"\"{kv.Key}\":{val}");
            }
            sb.Append("}}");

            yield return PostJson(
                $"{backendUrl}/sim/step",
                sb.ToString(),
                json =>
                {
                    SimTime = ParseFloat(json, "\"t\"");
                    ParseAllFloats(json, _outputs);
                    StatusMessage = $"t = {SimTime:F3} s";
                }
            );
        }

        private static async Task StopSessionAsync(string backendUrl, string sessionId)
        {
            try
            {
                using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(5) };
                using var content = new StringContent(
                    $"{{\"session_id\":\"{sessionId}\"}}",
                    Encoding.UTF8,
                    "application/json"
                );
                using var response = await client
                    .PostAsync($"{backendUrl}/sim/stop", content)
                    .ConfigureAwait(false);
            }
            catch
            {
                // Ignore shutdown errors; the scene is already closing.
            }
        }

        // ── HTTP ─────────────────────────────────────────────────────

        private IEnumerator PostJson(string url, string body, Action<string> onSuccess)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(body);
            using var req = new UnityWebRequest(url, "POST");
            req.uploadHandler = new UploadHandlerRaw(bytes);
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");
            req.timeout = 5;
            yield return req.SendWebRequest();
            if (req.result == UnityWebRequest.Result.Success)
                onSuccess(req.downloadHandler.text);
            else
                StatusMessage = $"HTTP error: {req.error}";
        }

        // ── JSON helpers ─────────────────────────────────────────────

        private static string ParseString(string json, string key)
        {
            string s = $"\"{key}\"";
            int i = json.IndexOf(s, StringComparison.Ordinal);
            if (i < 0)
                return string.Empty;
            i += s.Length;
            while (i < json.Length && (json[i] == ' ' || json[i] == ':'))
                i++;
            if (i >= json.Length || json[i] != '"')
                return string.Empty;
            i++;
            int end = json.IndexOf('"', i);
            return end < 0 ? string.Empty : json.Substring(i, end - i);
        }

        private static float ParseFloat(string json, string key)
        {
            int i = json.IndexOf(key, StringComparison.Ordinal);
            if (i < 0)
                return 0f;
            i += key.Length;
            while (i < json.Length && (json[i] == ' ' || json[i] == ':'))
                i++;
            int end = i;
            while (
                end < json.Length
                && (
                    char.IsDigit(json[end])
                    || json[end] == '.'
                    || json[end] == '-'
                    || json[end] == 'e'
                    || json[end] == 'E'
                    || json[end] == '+'
                )
            )
                end++;
            return float.TryParse(
                json.Substring(i, end - i),
                System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture,
                out float v
            )
                ? v
                : 0f;
        }

        private static void ParseAllFloats(string json, Dictionary<string, float> dict)
        {
            int start = json.IndexOf("\"outputs\"", StringComparison.Ordinal);
            string block = start >= 0 ? json.Substring(start) : json;
            int pos = 0;
            while (pos < block.Length)
            {
                int q1 = block.IndexOf('"', pos);
                if (q1 < 0)
                    break;
                int q2 = block.IndexOf('"', q1 + 1);
                if (q2 < 0)
                    break;
                string key = block.Substring(q1 + 1, q2 - q1 - 1);
                pos = q2 + 1;
                while (pos < block.Length && (block[pos] == ' ' || block[pos] == ':'))
                    pos++;
                if (pos >= block.Length)
                    break;
                if (block[pos] == '"' || block[pos] == '{' || block[pos] == '[')
                    continue;
                int end = pos;
                while (
                    end < block.Length
                    && (
                        char.IsDigit(block[end])
                        || block[end] == '.'
                        || block[end] == '-'
                        || block[end] == 'e'
                        || block[end] == 'E'
                        || block[end] == '+'
                    )
                )
                    end++;
                if (
                    end > pos
                    && float.TryParse(
                        block.Substring(pos, end - pos),
                        System.Globalization.NumberStyles.Float,
                        System.Globalization.CultureInfo.InvariantCulture,
                        out float v
                    )
                )
                    dict[key] = v;
                pos = end;
            }
        }
    }
}
