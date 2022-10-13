using Game.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public static UnityAction ActionGameStart, ActionGameOver, ActionMiniGame, ActionLevelPass, ActionLevelRestart;
    private int CurrentLevelIndex => SceneManager.GetActiveScene().buildIndex;

    public void LoadNextLevel()
    {
        int nextLevel = CurrentLevelIndex + 1;

        PlayerPrefs.SetInt("LEVEL", nextLevel);
        SceneManager.LoadScene(nextLevel);
    }

    public void RestartLevel()
    {
        ActionLevelRestart?.Invoke();
        SceneManager.LoadScene(CurrentLevelIndex);
    }

    public void CalculateTheProgress(float playerPosZ)
    {
        
    }

    private void PauseTheGame()
    {
        Time.timeScale = 0;
    }
}
