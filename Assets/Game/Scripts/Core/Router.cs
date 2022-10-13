using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Router : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(LoadTheGame());
    }

    private IEnumerator LoadTheGame()
    {
        yield return new WaitForSeconds(1.5f);

        int lastLevelIndex = PlayerPrefs.GetInt("LEVEL", 1);
        SceneManager.LoadScene(lastLevelIndex);
    }
}
