using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Managers/GameSettings")]
public class GameSettingsManager : ScriptableObject
{
    [Header("게임 설정")]
    public int maxLives = 3;
    public float enemySpawnRate = 2.0f;
    public string version = "1.0.0";

    public float spawnSpaceShipInterval = 10.0f;
    public float explorationInterval = 15.0f;


    public void ApplySettings()
    {
        Debug.Log("설정을 적용합니다.");
        // 예: AudioListener.volume = masterVolume;
    }
}
