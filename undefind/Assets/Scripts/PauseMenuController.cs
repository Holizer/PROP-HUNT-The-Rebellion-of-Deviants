using Photon.Pun;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    [Header("��������� ��������� ������")]
    [SerializeField] private PlayerStateManager playerStateManager;

    [Header("UI ��������")]
    public GameObject pauseMenuUI;
    public GameObject settingsMenuUI;

    [Header("��������� ����")]
    protected static bool isPaused = false;


    void Start()
    {
        if (playerStateManager == null)
        {
            playerStateManager = transform.parent.GetComponent<PlayerStateManager>();
        };

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
        SetCursorState(true, CursorLockMode.None);

        playerStateManager.PauseGame();
    }

    private void Resume()
    {
        isPaused = false;
        pauseMenuUI.SetActive(false);
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
        settingsMenuUI.SetActive(true);
    }

    public void OnExitToMainMenu()
    {
        PhotonNetwork.LeaveRoom();
        Loader.Load(Scene.Menu);
    }
}
