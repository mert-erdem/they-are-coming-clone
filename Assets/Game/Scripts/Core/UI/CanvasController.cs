using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : Singleton<CanvasController>
{
    [SerializeField] private GameObject panelMenu, panelInGame, panelMiniGame, panelEndGame;
    [SerializeField] private TextMeshProUGUI textPoints, textEndGamePoints, textIndicator;
    [SerializeField] private Image imageProgressBar;
    [SerializeField] private GameObject topPointIndicator;

    void Start()
    {
        GameManager.ActionGameStart += SetInGameUi;
        GameManager.ActionMiniGame += SetMiniGameUi;
        GameManager.ActionGameEnd += SetEndGameUi;
    }

    private void SetInGameUi()
    {
        panelMenu.SetActive(false);
        panelInGame.SetActive(true);
    }

    private void SetMiniGameUi()
    {
        panelInGame.SetActive(false);
        panelMiniGame.SetActive(true);
    }

    private void SetEndGameUi()
    {
        topPointIndicator.SetActive(false);
        panelMiniGame.SetActive(false);
        panelEndGame.SetActive(true);
    }

    #region UI Buttons' Methods
    public void ButtonStartPressed()
    {
        GameManager.ActionGameStart?.Invoke();
    }

    public void ButtonNextLevelPressed()
    {
        GameManager.Instance.LoadNextLevel();
    }

    public void ButtonReplayPressed()
    {
        GameManager.Instance.RestartLevel();
    }
    #endregion

    public void SetTextPoints(int points)
    {
        textPoints.text = points.ToString();
    }

    public void SetEndGameTextPoints(int points)
    {
        textEndGamePoints.text = points.ToString();
    }

    public void SetIndicatorText(int count)
    {
        textIndicator.text = count.ToString();
    }

    public void UpdateProgressBar(float value)
    {
        imageProgressBar.fillAmount = value;
    }

    private void OnDestroy()
    {
        GameManager.ActionGameStart -= SetInGameUi;
        GameManager.ActionMiniGame -= SetMiniGameUi;
        GameManager.ActionGameEnd -= SetEndGameUi;
    }
}
