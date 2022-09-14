using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : Singleton<CanvasController>
{
    [SerializeField] private GameObject panelMenu, panelInGame, panelGameOver, panelMiniGame, panelLevelPass;
    [SerializeField] private TextMeshProUGUI textPoints, textEndGamePoints, textIndicator;
    [SerializeField] private Image imageProgressBar;
    [SerializeField] private GameObject topPointIndicator;

    void Start()
    {
        GameManager.ActionGameStart += SetInGameUi;
        GameManager.ActionGameOver += SetGameOverUi;
        //GameManager.ActionMiniGame += SetMiniGameUi;
        GameManager.ActionLevelPass += SetLevelPassUi;
    }

    private void SetInGameUi()
    {
        panelMenu.SetActive(false);
        panelInGame.SetActive(true);
    }

    private void SetGameOverUi()
    {
        panelInGame.SetActive(false);
        panelGameOver.SetActive(true);
    }

    private void SetMiniGameUi()
    {
        panelInGame.SetActive(false);
        panelMiniGame.SetActive(true);
    }

    private void SetLevelPassUi()
    {
        //topPointIndicator.SetActive(false);
        //panelMiniGame.SetActive(false);
        panelInGame.SetActive(false);
        panelLevelPass.SetActive(true);
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

    public void ButtonRestartPressed()
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
        GameManager.ActionGameOver -= SetGameOverUi;
        GameManager.ActionMiniGame -= SetMiniGameUi;
        GameManager.ActionLevelPass -= SetLevelPassUi;
    }
}
