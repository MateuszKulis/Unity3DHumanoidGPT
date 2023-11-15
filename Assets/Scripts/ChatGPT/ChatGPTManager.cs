using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ChatGPTManager : MonoBehaviour
{
    private string openAIEndpoint = "https://api.openai.com/v1/engines/davinci-codex/completions";  
    private string openAIKey = "your_key";

    private void Awake()
    {
        GetUserInput("Who  are You?");
    }
    IEnumerator SendChatGPTRequest(string userMessage)
    {
        string requestData = "{\"prompt\": \"" + userMessage + "\", \"max_tokens\": 100}";

        var headers = new Dictionary<string, string>();
        headers.Add("Content-Type", "application/json");
        headers.Add("Authorization", "Bearer " + openAIKey);

        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(openAIEndpoint, requestData))
        {
            foreach (var header in headers)
            {
                www.SetRequestHeader(header.Key, header.Value);
            }

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError("Błąd API: " + www.error);
            }
            else
            {
                string response = www.downloadHandler.text;
                Debug.Log("Odpowiedź API: " + response);

            }
        }
    }

    public void GetUserInput(string userInput)
    {
        StartCoroutine(SendChatGPTRequest(userInput));
    }
}
