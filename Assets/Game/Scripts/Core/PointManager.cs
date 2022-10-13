using Game.Core;
using UnityEngine;

public class PointManager : Singleton<PointManager>
{
    private int totalPoints = 0;
    [SerializeField] private int pointsPerItem = 10;

    void Start()
    {
        GameManager.ActionLevelPass += SavePoints;

        totalPoints = PlayerPrefs.GetInt("POINTS", 0);
        // update ui
    }

    public void AddPoints()
    {
        totalPoints += pointsPerItem;
        // update ui
    }

    private void SavePoints()
    {
        // update ui
        PlayerPrefs.SetInt("POINTS", totalPoints);
    }

    private void OnDestroy()
    {
        GameManager.ActionLevelPass -= SavePoints;
    }
}
