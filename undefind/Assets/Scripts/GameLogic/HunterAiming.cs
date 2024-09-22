using UnityEngine;

public class HunterAiming : MonoBehaviour
{
    [Header("����������")]
    private ThirdPersonCamera camera; // ������ �� ������
    public Transform cameraRig; // ������ ��� � ��������
    public Transform player; // ����� ��� ��� ������ ��� ��������
    public Transform aimPosition; // ����� ������������
    public GameObject crosshair; // ������

    [Header("���������")]
    public float aimSpeed = 5f; // �������� ����������� ������
    public float rotationSpeed = 10f; // �������� �������� ������
    public float distanceBehindPlayer = 0.5f; // ���������� ������ ������
    public float shoulderOffset = 0.5f; // �������� � �����

    private bool isAiming = false;
    private void Start()
    {
        camera = cameraRig.GetComponent<ThirdPersonCamera>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // ������ ������ ���� ��� ������������
        {
            StartAiming();
        }

        if (Input.GetMouseButtonUp(1)) // ���������� ������ ������ ���� ��� ���������� ������������
        {
            StopAiming();
        }

        UpdateCameraPosition(); // ��������� ������� ������
    }

    void StartAiming()
    {
        isAiming = true;
        camera.SetAiming(true);
        if (crosshair != null)
        {
            crosshair.SetActive(true); // ���������� ������
        }
    }

    void StopAiming()
    {
        isAiming = false;
        camera.SetAiming(false);
        if (crosshair != null)
        {
            crosshair.SetActive(false); // �������� ������
        }
    }

    void UpdateCameraPosition()
    {
        if (cameraRig == null || aimPosition == null)
        {
            Debug.LogWarning("CameraRig ��� AimPosition �� ���������.");
            return;
        }

        if (isAiming)
        {
            // ������� ����������� ������ � ����� AimPosition
            cameraRig.position = Vector3.Lerp(cameraRig.position, aimPosition.position, Time.deltaTime * aimSpeed);
            cameraRig.rotation = Quaternion.Lerp(cameraRig.rotation, aimPosition.rotation, Time.deltaTime * aimSpeed);
        }
    }
}
