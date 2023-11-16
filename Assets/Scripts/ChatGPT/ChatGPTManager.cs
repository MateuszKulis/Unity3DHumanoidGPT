using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using System.Collections.Generic;

[System.Serializable]
public class ChatGPTResponse
{
    public List<ChatGPTChoice> choices;
}

[System.Serializable]
public class ChatGPTChoice
{
    public ChatGPTMessage message;
}

[System.Serializable]
public class ChatGPTMessage
{
    public string role;
    public string content;
}

[System.Serializable]
public class ChatGPTRequest
{
    public List<ChatGPTMessage> messages;
    public int max_tokens;
    public string model;
}

public class ChatGPTManager : MonoBehaviour
{
    [SerializeField] private string openAIEndpoint = "https://api.openai.com/v1/chat/completions";
    [SerializeField] private string openAIKey = "your api key";
    [TextArea(3, 10)][SerializeField] private string aiRole = "you are a lost fisherman who has lost his way home in the middle of the forest";
    [SerializeField] private TextMeshProUGUI playerTextArea;

    [SerializeField] private Transform chatContainer;
    [SerializeField] private GameObject userMessagePrefab;
    [SerializeField] private GameObject aiMessagePrefab;

    IEnumerator SendChatGPTRequest(string userMessage)
    {
        playerTextArea.text = "";

        ChatGPTRequest requestData = new ChatGPTRequest
        {
            messages = new List<ChatGPTMessage>
            {
                new ChatGPTMessage { role = "system", content = aiRole },
                new ChatGPTMessage { role = "user", content = userMessage }
            },
            max_tokens = 100,
            model = "gpt-3.5-turbo"
        };
        string jsonData = JsonUtility.ToJson(requestData);

        var headers = new Dictionary<string, string>();
        headers.Add("Content-Type", "application/json");
        headers.Add("Authorization", "Bearer " + openAIKey);

        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(openAIEndpoint, "POST"))
        {
            foreach (var header in headers)
            {
                www.SetRequestHeader(header.Key, header.Value);
            }

            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError("Błąd API: " + www.error + "\nOdpowiedź API: " + www.downloadHandler.text);
            }
            else
            {
                string response = www.downloadHandler.text;
                ChatGPTResponse responseData = JsonUtility.FromJson<ChatGPTResponse>(response);
                string content = responseData.choices[0].message.content;

                // Dodaj nowy prefab wiadomości do kontenera
                GameObject newMessage = Instantiate(aiMessagePrefab, chatContainer);
                newMessage.GetComponentInChildren<TextMeshProUGUI>().text = content;
                Debug.Log("Odpowiedź API: " + response);
            }
        }
    }

    public void GetUserInput()
    {
        string userInput = playerTextArea.text.ToString();

        GameObject newMessage = Instantiate(userMessagePrefab, chatContainer);
        newMessage.GetComponentInChildren<TextMeshProUGUI>().text = userInput;

        StartCoroutine(SendChatGPTRequest(userInput));
    }
}
