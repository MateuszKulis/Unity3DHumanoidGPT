using System;
using System.Collections;
using System.Text;
using Newtonsoft.Json;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class ElevenlabsAPI : MonoBehaviour
{
    [SerializeField]
    private string _voiceId;
    [SerializeField]
    private string _apiKey;
    [SerializeField]
    private string _apiUrl = "https://api.elevenlabs.io";

    private AudioClip _audioClip;


    public bool Streaming;

    [Range(0, 4)]
    public int LatencyOptimization;

    public AudioSource audioSource;
    [SerializeField]private Animator animator;


    public ElevenlabsAPI(string apiKey, string voiceId)
    {
        _apiKey = apiKey;
        _voiceId = voiceId;
    }

    public void GetAudio(string text)
    {
        SetVoiceSettings(text);
        
        Debug.Log(text);
    }

    private void SetVoiceSettings(string message)
    {
        var postData = new TextToSpeechRequest
        {
            text = message,
            model_id = "eleven_monolingual_v1"
        };


        var voiceSetting = new VoiceSettings
        {
            stability = 0,
            similarity_boost = 0,
            style = 0.5f,
            use_speaker_boost = true
        };

        postData.voice_settings = voiceSetting;
        animator.SetBool("IsPlayingSound", true);
        StartCoroutine(DoRequest(postData));
    }
    IEnumerator DoRequest(TextToSpeechRequest postData)
    {
       
        var json = JsonConvert.SerializeObject(postData);
        var uH = new UploadHandlerRaw(Encoding.ASCII.GetBytes(json));
        var stream = (Streaming) ? "/stream" : "";
        var url = $"{_apiUrl}/v1/text-to-speech/{_voiceId}{stream}?optimize_streaming_latency={LatencyOptimization}";
        var request = UnityWebRequest.PostWwwForm(url, json);
        var downloadHandler = new DownloadHandlerAudioClip(url, AudioType.MPEG);
        if (Streaming)
        {
            downloadHandler.streamAudio = true;
        }
        request.uploadHandler = uH;
        request.downloadHandler = downloadHandler;
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("xi-api-key", _apiKey);
        request.SetRequestHeader("Accept", "audio/mpeg");

        Debug.Log("URL: " + url);
        Debug.Log("Request JSON: " + json);

        Debug.Log("Request Result: " + request.result);
        Debug.Log("Request Error: " + request.error);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error downloading audio: " + request.error);
            yield break;
        }

        AudioClip audioClip = downloadHandler.audioClip;
        PlayDialogSound(audioClip);

        request.Dispose();
    }

    private void PlayDialogSound(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();

        Invoke("OnSoundComplete", audioSource.clip.length);
    }


    private void OnSoundComplete()
    {
        animator.SetBool("IsPlayingSound", false);
    }


    
}
