using Photon.Pun;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    [Header("��������� ��������� ������")]
    [SerializeField] private PlayerStateManager playerStateManager;

    [Header("UI ��������")]
    [SerializeField] private UIManager uiManager;

    [Header("��������� ����")]
    protected static bool isPaused = false;

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
        uiManager.ShowPauseMenu(); 
        SetCursorState(true, CursorLockMode.None);

        playerStateManager.PauseGame();
    }

    private void Resume()
    {
        isPaused = false;
        uiManager.HidePauseMenu(); 
        SetCursorState(false, CursorLockMode.Locked);

        playerStateManager.ResumeGame();
    }

    private void SetCursorState(bool visible, CursorLockMode cursorLockMode)
    {
        Cursor.visible = visible;
        Cursor.lockState = cursorLockMode;
    }

    public void OnContinue()
    {
        Resume();
    }

    public void OnSettings()
    {
        uiManager.ShowSettingsMenu();
    }

    public void OnExitToMainMenu()
    {
        PhotonNetwork.LeaveRoom();
        Loader.Load(Scene.Menu);
    }
}
