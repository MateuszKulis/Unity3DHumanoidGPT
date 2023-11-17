using System;

[Serializable]
public class TextToSpeechRequest
{
    public string text;
    public string model_id;
    public VoiceSettings voice_settings;
}

[Serializable]
public class VoiceSettings
{
    public int stability;
    public int similarity_boost;
    public float style;
    public bool use_speaker_boost;
}