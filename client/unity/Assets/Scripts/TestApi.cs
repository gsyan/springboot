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
        // 회원가입
        yield return StartCoroutine(RunAsync(() => authManager.SignUpAsync("user1@example.com", "mySecurePassword")));

        // 로그인
        yield return StartCoroutine(RunAsync(() => authManager.LoginAsync("user1@example.com", "mySecurePassword")));

        // 캐릭터 생성
        yield return StartCoroutine(RunAsync(() => authManager.CreateCharacterAsync("Hero1")));

        // 캐릭터 조회
        //yield return StartCoroutine(RunAsync(() => authManager.GetCharacter(72057594037927936)));

        // 토큰 갱신
        yield return StartCoroutine(RunAsync(() => authManager.RefreshAccessTokenAsync()));

        // 로그아웃
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