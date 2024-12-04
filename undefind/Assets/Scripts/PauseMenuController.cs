using Photon.Pun;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject settingsMenuUI;

    protected static bool isPaused = false;

    [Tooltip("Отключить указанные компоненты во время меню паузы.")]
    public MonoBehaviour[] componentsToDisable;
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    private void Pause()
    {

        isPaused = true;
        pauseMenuUI.SetActive(true);

        ToggleComponentsEnabledState(false);
        SetCursorState(true, CursorLockMode.None);
    }

    private void Resume()
    {

        isPaused = false;
        pauseMenuUI.SetActive(false);
        
        SetCursorState(false, CursorLockMode.Locked);
        ToggleComponentsEnabledState(true);
    }

    private void SetCursorState(bool visible, CursorLockMode cursorLockMode)
    {
        Cursor.visible = visible;
        Cursor.lockState = cursorLockMode;
    }
    private void ToggleComponentsEnabledState(bool isEnable)
    {
        foreach (var component in componentsToDisable)
        {
            component.enabled = isEnable;
        }
    }
    public void OnContinue()
    {
        Resume();
    }

    public void OnSettings()
    {
        settingsMenuUI.SetActive(true);
    }

    public void OnExitToMainMenu()
    {
        PhotonNetwork.LeaveRoom();
        Loader.Load(Scene.Menu);
    }
}
