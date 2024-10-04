using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Префабы")]
    public GameObject playerPrefab;
    public GameObject hunterModelPrefab;
    public GameObject hiderModelPrefab;

    private GameObject currentPlayer;
    private GameObject currentPlayerModel;

    void Start()
    {
        PlayerRole selectedRole = ChooseRandomRole();
        Vector3 spawnPosition = GetSpawnPosition();
        SpawnPlayer(PlayerRole.Hunter, spawnPosition);
    }

    private PlayerRole ChooseRandomRole()
    {
        return (PlayerRole)Random.Range(0, System.Enum.GetValues(typeof(PlayerRole)).Length);
    }

    private void SpawnPlayer(PlayerRole role, Vector3 spawnPosition)
    {
        currentPlayer = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
        currentPlayerModel = InstantiatePlayerModel(role, currentPlayer.transform);

        CharacterController characterController = currentPlayer.GetComponent<CharacterController>();
        if (characterController != null)
        {
            characterController.enabled = true;
        }
        InitializePlayerScript(role);

        switch (role)
        {
            case PlayerRole.Hider:
                ConfigureHiderMovement(currentPlayerModel, characterController);
                break;
            case PlayerRole.Hunter:
                ConfigureHunterMovement(currentPlayerModel, characterController);
                break;
        }

        playerPrefab.SetActive(false);
        playerPrefab = null;
    }

    private GameObject InstantiatePlayerModel(PlayerRole role, Transform parent)
    {
        GameObject modelPrefab = role == PlayerRole.Hunter ? hunterModelPrefab : hiderModelPrefab;
        GameObject model = Instantiate(modelPrefab, parent);

        // Устанавливаем локальную позицию и вращение модели
        model.transform.localPosition = Vector3.zero;
        model.transform.localRotation = Quaternion.identity;

        return model;
    }

    private void InitializePlayerScript(PlayerRole role)
    {
        Player playerScript = currentPlayer.GetComponent<Player>();
        if (playerScript != null)
        {
            playerScript.SetRole(role);
            playerScript.SetLocalPlayer(true);

            if (role == PlayerRole.Hunter)
            {
                Hunter hunter = currentPlayer.AddComponent<Hunter>();
                if (hunter != null)
                {
                    hunter.Initialize(currentPlayerModel);
                }
            }
            else if (role == PlayerRole.Hider)
            {
                Hider hider = currentPlayer.AddComponent<Hider>();
                if (hider != null)
                {
                    hider.Initialize(currentPlayerModel);
                }
                else
                {
                    Debug.LogError("Не удалось создать Hider компонент на игроке!");
                }
            }
        }
    }

    private void ConfigureHiderMovement(GameObject model, CharacterController characterController)
    {
        Animator animator = model.GetComponent<Animator>();
        if (animator != null)
        {
            animator.enabled = true;
        }
        HiderAnimations hiderAnimations = model.GetComponent<HiderAnimations>();
        if (hiderAnimations != null)
        {
            hiderAnimations.enabled = true;
        }

        Transform cameraRig = currentPlayer.transform.Find("CameraRig");
        HiderMovement hiderMovement = model.GetComponent<HiderMovement>();
        if (hiderMovement != null)
        {
            hiderMovement.cameraTransform = cameraRig;
            hiderMovement.controller = characterController;
            hiderMovement.enabled = true;
        }

        ThirdPersonCamera thirdPersonCamera = cameraRig.GetComponent<ThirdPersonCamera>();
        thirdPersonCamera.player = currentPlayer.transform;
        thirdPersonCamera.enabled = true;

        GameObject aimPosition = currentPlayer.transform.Find("AimPosition").gameObject;
        Destroy(aimPosition);
    }
    private void ConfigureHunterMovement(GameObject model, CharacterController characterController)
    {
        Animator animator = model.GetComponent<Animator>();
        if (animator != null)
        {
            animator.enabled = true;
        }
        HunterAnimation hunterAnimation = model.GetComponent<HunterAnimation>();
        if (hunterAnimation != null)
        {
            hunterAnimation.enabled = true;
        }

        Transform cameraRig = currentPlayer.transform.Find("CameraRig");
        HunterMovement hunterMovement = model.GetComponent<HunterMovement>();
        if (hunterMovement != null)
        {
            hunterMovement.cameraTransform = cameraRig;
            hunterMovement.controller = characterController;
            hunterMovement.enabled = true;
        }

        ThirdPersonCamera thirdPersonCamera = cameraRig.GetComponent<ThirdPersonCamera>();
        thirdPersonCamera.player = currentPlayer.transform;
        thirdPersonCamera.enabled = true;

        Transform aimPosition = currentPlayer.transform.Find("AimPosition");
        if (aimPosition != null)
        {
            HunterAiming hunterAiming = aimPosition.GetComponent<HunterAiming>();
            if (hunterAiming != null)
            {
                hunterAiming.player = currentPlayer.transform;
                hunterAiming.camera = thirdPersonCamera;
                hunterAiming.aimPosition = aimPosition;
                hunterAiming.enabled = true;
            }
            else
            {
                Debug.LogError("HunterAiming не найден в AimPosition!");
            }
        }
        else
        {
            Debug.LogError("AimPosition не найден в текущем игроке!");
        }
    }

    private Vector3 GetSpawnPosition()
    {
        return new Vector3(Random.Range(-20, 20), 0, Random.Range(-20, 20));
    }

    // Метод для вызова при выборе роли через UI или другой механизм
    public void OnRoleSelected(PlayerRole selectedRole)
    {
    }
}
