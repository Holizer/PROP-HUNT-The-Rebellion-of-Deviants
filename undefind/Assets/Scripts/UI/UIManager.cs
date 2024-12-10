using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject endGamePanel;
    [SerializeField] private TextMeshProUGUI endGameText;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject settingsMenuUI;

    public void DisplayMessage(string message, Color color)
    {
        endGamePanel.SetActive(true);
        endGameText.text = message;      
        endGameText.color = color; 
    }

    public void HideMessage()
    {
        endGamePanel.SetActive(false);
        ScreenFader screenFader = endGamePanel.GetComponent<ScreenFader>();
        if (screenFader != null && screenFader.fadeImage != null)
        {
            Color color = screenFader.fadeImage.color;
            color.a = 0f;
            screenFader.fadeImage.color = color;

            screenFader.FadeIn();
        }
    }

    public void ShowPauseMenu()
    {
        pauseMenuUI.SetActive(true);
    }

    public void HidePauseMenu()
    {
        pauseMenuUI.SetActive(false);
    }

    public void ShowSettingsMenu()
    {
        if (settingsMenuUI.activeSelf)
        {
            settingsMenuUI.SetActive(false);
        }
        else
        {
            settingsMenuUI.SetActive(true);
        }
    }

    public void HideSettingsMenu()
    {
        settingsMenuUI.SetActive(false);
    }
}