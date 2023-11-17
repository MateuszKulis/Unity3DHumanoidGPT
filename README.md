# Unity Voice and Dialogue System Documentation

## 1. ElevenlabsAPI
Description

ElevenlabsAPI is a class in the system for handling requests and playing sounds using the ElevenLabs API.
### Attributes

   * _voiceId (string): The voice identifier used in the ElevenLabs API.
   * _apiKey (string): The API key needed for authenticating requests to the ElevenLabs API.
   * _apiUrl (string): The URL address of the ElevenLabs API.
   * _audioClip (AudioClip): Stores the played sound.

### Methods

   * ElevenlabsAPI(string apiKey, string voiceId): Class constructor, initializes the API key and voice identifier.
   * GetAudio(string text): Sends a request to the ElevenLabs API to get sound based on the provided text.
   * DoRequest(string message): Executes an HTTP request to the ElevenLabs API and plays the sound upon receiving a response.

## 2. ChatGPTManager
Description

ChatGPTManager handles communication with the GPT API to generate responses based on user input.
### Attributes

   * openAIEndpoint (string): The GPT API URL.
   * openAIKey (string): The API key for authenticating requests to the GPT API.
   * aiRole (string): The AI role used in API requests.
   * aiTextPrefab (TextMeshProUGUI): Prefabricated object for displaying text generated by the AI.
   * playerTextArea (TextMeshProUGUI): Player's text input area.

### Methods

   * SendChatGPTRequest(string userMessage): Sends a request to the GPT API to get a response based on the player's message.
   * GetUserInput(): Retrieves text from the player and initiates the response generation process.
