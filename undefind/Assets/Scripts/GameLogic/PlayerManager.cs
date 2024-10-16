using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerManager : MonoBehaviour
{
    [Header("�������")]
    public GameObject playerPrefab;
    public GameObject hunterModelPrefab;
    public GameObject hiderModelPrefab;

    public GameObject spawnArea;

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
                    Debug.LogError("�� ������� ������� Hider ��������� �� ������!");
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

        Transform rigTransform = currentPlayerModel.transform.Find("Rig");
        if (rigTransform != null)
        {
            Rig rigComponent = rigTransform.GetComponent<Rig>();
            if (rigComponent != null)
            {
                thirdPersonCamera.GetComponent<ThirdPersonCamera>().animationRig = rigComponent;
            }
        }

        HunterShoot hunterShoot = model.GetComponent<HunterShoot>();
        if (hunterShoot != null)
        {
            hunterShoot.camera = thirdPersonCamera;
            hunterShoot.enabled = true;
        }

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
                Debug.LogError("HunterAiming �� ������ � AimPosition!");
            }
        }
        else
        {
            Debug.LogError("AimPosition �� ������ � ������� ������!");
        }
    }

    private Vector3 GetSpawnPosition()
    {
        Bounds bounds = spawnArea.GetComponent<BoxCollider>().bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);
        float z = Random.Range(bounds.min.z, bounds.max.z);
        return new Vector3(x, y, z);
    }

    // ����� ��� ������ ��� ������ ���� ����� UI ��� ������ ��������
    public void OnRoleSelected(PlayerRole selectedRole)
    {
    }
}
