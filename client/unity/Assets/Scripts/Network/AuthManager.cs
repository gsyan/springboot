//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static ApiClient;

public class AuthManager : MonoBehaviour
{
    private ApiClient m_apiClient;
    
    private string m_refreshToken;
    private int m_characterCreateRetryCount = 0;
    private const int m_characterCreateMaxRetries = 2;


    void Awake()
    {
        m_apiClient = GetComponent<ApiClient>();
        if (m_apiClient == null)
            m_apiClient = gameObject.AddComponent<ApiClient>();

        // PlayerPrefs에서 리프레시 토큰 복원
        m_refreshToken = PlayerPrefs.GetString("RefreshToken", "");
    }

    public async Task<ApiResponse<String>> SignUpAsync(string email, string password)
    {
        try
        {
            var response = await m_apiClient.SignUpAsync(email, password);
            if (response.errorCode == 0)
            {
                Debug.Log($"Signed up: {response.data}");
            }
            else
            {
                Debug.LogError($"SignUp failed: {response.errorMessage} (Code: {response.errorCode})");
                throw new Exception(response.errorMessage);
            }
            return response;
        }
        catch (Exception e)
        {
            Debug.LogError($"SignUp failed: {e.Message}");
            throw;
        }
    }

    public async Task<ApiResponse<AuthResponse>> LoginAsync(string email, string password)
    {
        try
        {
            var response = await m_apiClient.LoginAsync(email, password);
            if (response.errorCode == 0)
            {
                m_apiClient.SetAccessToken(response.data.accessToken);
                m_refreshToken = response.data.refreshToken;
                PlayerPrefs.SetString("RefreshToken", m_refreshToken);
                PlayerPrefs.Save();
                Debug.Log($"Logged in: Access Token: {response.data.accessToken}");
            }
            else
            {
                Debug.LogError($"Login failed: {response.errorMessage} (Code: {response.errorCode})");
                throw new Exception(response.errorMessage);
            }
            return response;
        }
        catch (Exception e)
        {
            Debug.LogError($"Login failed: {e.Message}");
            throw;
        }
    }

    public async Task<ApiResponse<AuthResponse>> RefreshAccessTokenAsync()
    {
        try
        {
            if (string.IsNullOrEmpty(m_refreshToken))
            {
                throw new Exception("No refresh token available");
            }
            var response = await m_apiClient.RefreshTokenAsync(m_refreshToken);
            if (response.errorCode == 0)
            {
                m_apiClient.SetAccessToken(response.data.accessToken);
                m_refreshToken = response.data.refreshToken;
                PlayerPrefs.SetString("RefreshToken", m_refreshToken);
                PlayerPrefs.Save();
                Debug.Log($"Token refreshed: New Access Token: {response.data.accessToken}");
            }
            else
            {
                Debug.LogError($"Token refresh failed: {response.errorMessage} (Code: {response.errorCode})");
                throw new Exception(response.errorMessage);
            }
            return response;
        }
        catch (Exception e)
        {
            Debug.LogError($"Token refresh failed: {e.Message}");
            throw;
        }
    }

    public void Logout()
    {
        PlayerPrefs.DeleteKey("RefreshToken");
        PlayerPrefs.Save();
        m_apiClient.SetAccessToken(null);
        m_refreshToken = null;
        Debug.Log("Logged out");
    }


    public async Task<ApiResponse<CharacterResponse>> CreateCharacterAsync(string characterName)
    {
        try
        {
            var response = await m_apiClient.CreateCharacterAsync(characterName);
            if (response == null)
            {
                Debug.LogError("Response is null");
                return ApiResponse<CharacterResponse>.error((int)ServerErrorCode.CHARACTER_CREATE_FAIL_REASON1, "Invalid server response");
            }
            if (response.errorCode == 0)
            {
                Debug.Log($"Character created: {response.data.characterName}, ID: {response.data.characterId}");
            }
            else
            {
                Debug.LogError($"Character creation failed: {response.errorMessage} (Code: {response.errorCode})");
            }
            return response;
        }
        catch (Exception e)
        {
            Debug.LogError($"Character creation failed: {e.Message}");
            if (e.Message.Contains("401") && m_characterCreateRetryCount < m_characterCreateMaxRetries)
            {
                m_characterCreateRetryCount++;
                await RefreshAccessTokenAsync();
                return await CreateCharacterAsync(characterName); // 재시도 후 결과 반환
            }
            m_characterCreateRetryCount = 0; // 재시도 실패 또는 최대 횟수 초과 시 초기화
            return ApiResponse<CharacterResponse>.error((int)ServerErrorCode.UNKNOWN_ERROR, e.Message); // 에러 응답 반환
        }
    }

    public async Task<ApiResponse<List<CharacterResponse>>> GetAllCharactersAsync()
    {
        try
        {
            var response = await m_apiClient.GetAllCharactersAsync();
            if (response == null)
            {
                Debug.LogError("Response is null");
                return ApiResponse<List<CharacterResponse>>.error((int)ServerErrorCode.UNKNOWN_ERROR, "Invalid server response");
            }
            if (response.errorCode == 0)
            {
                Debug.Log("Characters retrieved successfully");
                foreach (var character in response.data)
                {
                    Debug.Log($"Character: {character.characterName}, ID: {character.characterId}");
                }
            }
            else
            {
                Debug.LogError($"GetAllCharacters failed: {response.errorMessage} (Code: {response.errorCode})");
                throw new Exception(response.errorMessage);
            }
            return response;
        }
        catch (Exception e)
        {
            Debug.LogError($"GetAllCharacters failed: {e.Message}");
            throw;
        }
    }

}