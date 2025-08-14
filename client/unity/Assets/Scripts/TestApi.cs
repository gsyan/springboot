//------------------------------------------------------------------------------
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class TestApi : MonoBehaviour
{
    private AuthManager authManager;

    void Start()
    {
        authManager = GetComponent<AuthManager>();
        StartCoroutine(TestApiCalls());
    }

    private IEnumerator TestApiCalls()
    {
        // ȸ������
        yield return StartCoroutine(RunAsync(() => authManager.SignUpAsync("user1@example.com", "mySecurePassword")));

        // �α���
        yield return StartCoroutine(RunAsync(() => authManager.LoginAsync("user1@example.com", "mySecurePassword")));

        // ĳ���� ����
        yield return StartCoroutine(RunAsync(() => authManager.CreateCharacterAsync("Hero1")));

        // ĳ���� ��ȸ
        //yield return StartCoroutine(RunAsync(() => authManager.GetCharacter(72057594037927936)));

        // ��ū ����
        yield return StartCoroutine(RunAsync(() => authManager.RefreshAccessTokenAsync()));

        // �α׾ƿ�
        authManager.Logout();
    }

    private IEnumerator RunAsync(System.Func<Task> taskFunc)
    {
        Task task = taskFunc();
        while (!task.IsCompleted)
        {
            yield return null;
        }
        if (task.IsFaulted)
        {
            Debug.LogError($"Task failed: {task.Exception?.InnerException?.Message}");
        }
    }
}