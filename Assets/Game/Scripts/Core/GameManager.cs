using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public static UnityAction ActionGameStart, ActionMiniGame, ActionGameEnd;

    public void LoadNextLevel()
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        int nextLevel = currentLevelIndex + 1;

        PlayerPrefs.SetInt("LEVEL", nextLevel);
        SceneManager.LoadScene(currentLevelIndex + 1);
    }

    public void RestartLevel()
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentLevelIndex);
    }

    public void CalculateTheProgress(float playerPosZ)
    {
        
    }
}
