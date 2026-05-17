using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class RAGRequest
{
    public string query;
    public string focus_component;
    public string display_name; // ← new: sent as extra context hint
}

[Serializable]
public class RAGResponse
{
    public string answer;
    public string[] follow_ups;
}

public class ChamuscaAPIClient : MonoBehaviour
{
    public string backendUrl = "http://127.0.0.1:8000/query";

    public void SendQuery(
        string query,
        string focusComponent,
        Action<RAGResponse> onSuccess,
        Action<string> onError,
        string displayName = ""
    ) // ← new optional param
    {
        StartCoroutine(PostQuery(query, focusComponent, displayName, onSuccess, onError));
    }

    private IEnumerator PostQuery(
        string queryText,
        string focusComponent,
        string displayName,
        Action<RAGResponse> onSuccess,
        Action<string> onError
    )
    {
        // If a display name is known, prepend it so vector search steers correctly.
        // e.g. "Reóstato: como funciona?" instead of just "como funciona?"
        string enrichedQuery = !string.IsNullOrEmpty(displayName)
            ? $"{displayName}: {queryText}"
            : queryText;

        RAGRequest req = new RAGRequest
        {
            query = enrichedQuery,
            focus_component = focusComponent,
            display_name = displayName,
        };

        string jsonBody = JsonUtility.ToJson(req);

        using (UnityWebRequest request = new UnityWebRequest(backendUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (
                request.result == UnityWebRequest.Result.ConnectionError
                || request.result == UnityWebRequest.Result.ProtocolError
            )
            {
                onError?.Invoke(request.error);
            }
            else
            {
                try
                {
                    RAGResponse res = JsonUtility.FromJson<RAGResponse>(
                        request.downloadHandler.text
                    );
                    onSuccess?.Invoke(res);
                }
                catch (Exception e)
                {
                    onError?.Invoke("JSON Parse error: " + e.Message);
                }
            }
        }
    }
}
