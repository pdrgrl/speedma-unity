# Chamusca Unity Client - Phase 2

This folder contains the scripts for connecting Unity to the Python RAG Backend.

## 1. ChamuscaAPIClient.cs
Attach this script to an empty GameObject (e.g., `ChamuscaAPI`).
It uses `UnityWebRequest` to send JSON payloads to your local FastAPI server (`http://127.0.0.1:8000/query`).
It exposes a clean `SendQuery` method that takes care of the JSON serialization and coroutine execution.

## 2. ChamuscaUIManager.cs
Attach this script to your Canvas or a UI Manager GameObject.
Drag your UI elements into the exposed Inspector slots:
- `InputField`: (TMP_InputField) where the user types.
- `SendButton`: (Button) to trigger the query.
- `AnswerText`: (TMP_Text) to display the Gemini answer.
- `FollowUpButtons`: Array of 3 Buttons to display the suggested follow-ups from the RAG.

It will handle disabling buttons while "Thinking...", populating the text, and wiring the follow-up buttons automatically.
