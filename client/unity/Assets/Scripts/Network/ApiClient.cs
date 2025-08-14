//------------------------------------------------------------------------------
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class ApiClient : MonoBehaviour
{
    private readonly string baseUrl = "http://localhost:8080/api";
    private string accessToken;

    [System.Serializable]
    public class SignUpRequest
    {
        public string email;
        public string password;
    }

    [System.Serializable]
    public class LoginRequest
    {
        public string email;
        public string password;
    }

    [System.Serializable]
    public class AuthResponse
    {
        public string accessToken;
        public string refreshToken;
    }

    [System.Serializable]
    public class RefreshTokenRequest
    {
        public string refreshToken;
    }

    [System.Serializable]
    public class CharacterCreateRequest
    {
        public string characterName;
    }

    [System.Serializable]
    public class CharacterResponse
    {
        public long id;
        public long characterId;
        public string characterName;
        public string dateTime;
    }

    public void SetAccessToken(string token)
    {
        accessToken = token;
        Debug.Log($"SetAccessToken: {accessToken}");
    }

    [System.Serializable]
    public class ApiResponse<T>
    {
        public int errorCode;
        public string errorMessage;
        public T data;

        public static ApiResponse<T> success(T data)
        {
            return new ApiResponse<T> { errorCode = 0, errorMessage = "Success", data = data };
        }

        public static ApiResponse<T> error(int code, string message)
        {
            return new ApiResponse<T> { errorCode = code, errorMessage = message, data = default };
        }
    }

    private async Task SendRequestAsync(UnityWebRequest request)
    {
        var operation = request.SendWebRequest();
        while (!operation.isDone)
        {
            await Task.Yield();
        }

        if (request.result != UnityWebRequest.Result.Success)
        {
            string errorText = request.downloadHandler?.text ?? request.error;
            int errorCode = request.responseCode == 401 ? (int)ServerErrorCode.LOGIN_FAIL_REASON1 : (int)ServerErrorCode.UNKNOWN_ERROR;
            Debug.LogError($"Request failed: {request.error} - {errorText}");
            throw new Exception($"Request failed: {errorText} (Code: {errorCode})");
        }
    }

    public async Task<ApiResponse<string>> SignUpAsync(string email, string password)
    {
        var requestDto = new SignUpRequest { email = email, password = password };
        string json = JsonConvert.SerializeObject(requestDto);
        Debug.Log($"SignUp JSON: {json}");

        using var request = new UnityWebRequest($"{baseUrl}/account/signup", "POST");
        request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        await SendRequestAsync(request);
        return JsonConvert.DeserializeObject<ApiResponse<string>>(request.downloadHandler.text);
    }

    public async Task<ApiResponse<AuthResponse>> LoginAsync(string email, string password)
    {
        var requestDto = new LoginRequest { email = email, password = password };
        string json = JsonConvert.SerializeObject(requestDto);
        Debug.Log($"Login JSON: {json}");

        using var request = new UnityWebRequest($"{baseUrl}/account/login", "POST");
        request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        await SendRequestAsync(request);
        return JsonConvert.DeserializeObject<ApiResponse<AuthResponse>>(request.downloadHandler.text);
    }

    public async Task<ApiResponse<AuthResponse>> RefreshTokenAsync(string refreshToken)
    {
        var requestDto = new RefreshTokenRequest { refreshToken = refreshToken };
        string json = JsonConvert.SerializeObject(requestDto);
        Debug.Log($"RefreshToken JSON: {json}");

        using var request = new UnityWebRequest($"{baseUrl}/account/refresh", "POST");
        request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        await SendRequestAsync(request);
        return JsonConvert.DeserializeObject<ApiResponse<AuthResponse>>(request.downloadHandler.text);
    }

    public async Task<ApiResponse<CharacterResponse>> CreateCharacterAsync(string characterName)
    {
        if (string.IsNullOrEmpty(accessToken))
        {
            Debug.LogError("AccessToken is null or empty");
            throw new Exception("AccessToken is not set");
        }

        var requestDto = new CharacterCreateRequest { characterName = characterName };
        string json = JsonConvert.SerializeObject(requestDto);
        Debug.Log($"CreateCharacter JSON: {json}");

        using var request = new UnityWebRequest($"{baseUrl}/character/create", "POST");
        request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", $"Bearer {accessToken}");

        await SendRequestAsync(request);
        return JsonConvert.DeserializeObject<ApiResponse<CharacterResponse>>(request.downloadHandler.text);
    }

    

    public async Task<ApiResponse<List<CharacterResponse>>> GetAllCharactersAsync()
    {
        if (string.IsNullOrEmpty(accessToken))
        {
            Debug.LogError("AccessToken is null or empty");
            throw new Exception("AccessToken is not set");
        }

        using var request = new UnityWebRequest($"{baseUrl}/account/characters", "GET");
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", $"Bearer {accessToken}");

        await SendRequestAsync(request);
        return JsonConvert.DeserializeObject<ApiResponse<List<CharacterResponse>>>(request.downloadHandler.text);
    }

}


