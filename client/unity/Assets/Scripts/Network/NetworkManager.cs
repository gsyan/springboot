//------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using static ApiClient;

public static class TaskExtensions
{
    public static IEnumerator WrapToCoroutine(this Task task)
    {
        while (!task.IsCompleted)
            yield return null;
        if (task.IsFaulted)
            Debug.LogError($"Task failed: {task.Exception}");
    }
}

public class NetworkManager : MonoSingleton<NetworkManager>
{
    #region MonoSingleton ---------------------------------------------------------------
    protected override void OnInitialize()
    {
        m_authManager = GetComponent<AuthManager>();
        if (m_authManager == null)
            m_authManager = gameObject.AddComponent<AuthManager>();

        if (SceneManager.GetActiveScene().name == "MainScene")
            GameObject.Find("UIMain")?.TryGetComponent(out m_uIManager);
        else if (SceneManager.GetActiveScene().name == "SpaceScene")
            GameObject.Find("UISpace")?.TryGetComponent(out m_uIManager);
    }
    #endregion

    private AuthManager m_authManager;
    private UIManager m_uIManager;

    private NetworkReachability m_networkStatus;
    private bool m_bConnected = false;
    private Queue<Action> m_pendingRequests = new Queue<Action>();

    void Start()
    {
        // ��׶��忡���� �۵��� ���� : ���� thread ����,
        InvokeRepeating(nameof(CheckConnection), 0f, 10f); // 10�ʸ��� üũ
    }

    void CheckConnection()
    {
        m_networkStatus = Application.internetReachability;
        if (m_networkStatus == NetworkReachability.NotReachable)
        {
            Debug.Log("��Ʈ��ũ ����");
            m_bConnected = false;
        }
        else
        {
            Debug.Log(m_networkStatus + " �� ���ͳ� ������ - �翬�� �õ�");
            StartCoroutine(CheckInternetAccess());
            //if (m_networkStatus == NetworkReachability.ReachableViaCarrierDataNetwork)
            //else if(m_networkStatus == NetworkReachability.ReachableViaLocalAreaNetwork)
        }
    }

    // ���ͳ��� ���� �Ǵ��� Ȯ��
    IEnumerator CheckInternetAccess()
    {
        using (UnityEngine.Networking.UnityWebRequest request =
            UnityEngine.Networking.UnityWebRequest.Get("https://www.google.com"))
        {
            request.timeout = 3; // 3�� ����
            yield return request.SendWebRequest();

            if (request.result == UnityEngine.Networking.UnityWebRequest.Result.Success)
            {
                Debug.Log("���ͳ� ���� ���� m_networkStatus: " + m_networkStatus);
                if (m_bConnected == false)
                    m_bConnected = true;
                yield return StartCoroutine(ProcessPendingRequestsAsync().WrapToCoroutine());
            }
            else
            {
                Debug.Log("���ͳ� ������ ���� m_networkStatus: " + m_networkStatus);
            }
        }
    }
    

    private async Task ProcessPendingRequestsAsync()
    {
        while (m_pendingRequests.Count > 0)
        {
            try
            {
                var action = m_pendingRequests.Dequeue();
                await Task.Run(action); // �񵿱� ����
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to process queued request: {e.Message}");
            }
        }
    }


    private IEnumerator RunAsync<T>(Func<Task<ApiResponse<T>>> taskFunc)
    {
        m_uIManager.m_resultText.text = "Processing...";
        Task<ApiResponse<T>> task = taskFunc();
        while (!task.IsCompleted)
        {
            yield return null;
        }
        if (task.IsFaulted)
        {
            string errorMessage = task.Exception?.InnerException?.Message ?? "Unknown error";
            Debug.LogError($"Task failed: {errorMessage} - StackTrace: {task.Exception?.StackTrace}");
            m_uIManager.m_resultText.text = $"Error: {errorMessage}";
            yield break; // ���� �߻� �� �ڷ�ƾ ����
        }
        var response = task.Result;
        if (response != null)
        {
            if (response.errorCode == 0)
            {
                Debug.Log($"Operation succeeded: {typeof(T).Name} retrieved");
                m_uIManager.m_resultText.text = $"Operation succeeded: {typeof(T).Name} retrieved";
            }
            else
            {
                Debug.LogError($"Operation failed: {response.errorMessage} (Code: {response.errorCode})");
                m_uIManager.m_resultText.text = $"Operation failed: {response.errorMessage} (Code: {response.errorCode})";
            }
        }
    }

    public void Register(string email, string password)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            m_uIManager.m_resultText.text = "Email and password are required!";
            return;
        }

        if (!m_bConnected)
        {
            m_pendingRequests.Enqueue(() => StartCoroutine(RunAsync(() => m_authManager.SignUpAsync(email, password))));
            return;
        }
        StartCoroutine(RunAsync(() => m_authManager.SignUpAsync(email, password)));
    }

    public void Login(string email, string password)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            m_uIManager.m_resultText.text = "Email and password are required!";
            return;
        }

        StartCoroutine(RunAsync(() => m_authManager.LoginAsync(email, password)));
    }

    public void CreateCharacter(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            m_uIManager.m_resultText.text = "Character name is required!";
            return;
        }

        StartCoroutine(RunAsync(() => m_authManager.CreateCharacterAsync(name)));
    }

    public void GetCharacters()
    {
        StartCoroutine(RunAsync(() => m_authManager.GetAllCharactersAsync()));
    }

}
