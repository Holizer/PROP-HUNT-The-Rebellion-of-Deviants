using UnityEngine;

public class HunterAiming : MonoBehaviour
{
    [Header("Компоненты")]
    private ThirdPersonCamera camera; // Ссылка на камеру
    public Transform cameraRig; // Камера или её родитель
    public Transform player; // Игрок или его объект для вращения
    public Transform aimPosition; // Точка прицеливания
    public GameObject crosshair; // Прицел

    [Header("Настройки")]
    public float aimSpeed = 5f; // Скорость перемещения камеры
    public float rotationSpeed = 10f; // Скорость поворота игрока
    public float distanceBehindPlayer = 0.5f; // Расстояние позади игрока
    public float shoulderOffset = 0.5f; // Смещение к плечу

    private bool isAiming = false;
    private void Start()
    {
        camera = cameraRig.GetComponent<ThirdPersonCamera>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Правая кнопка мыши для прицеливания
        {
            StartAiming();
        }

        if (Input.GetMouseButtonUp(1)) // Отпускание правой кнопки мыши для завершения прицеливания
        {
            StopAiming();
        }

        UpdateCameraPosition(); // Обновляем позицию камеры
    }

    void StartAiming()
    {
        isAiming = true;
        camera.SetAiming(true);
        if (crosshair != null)
        {
            crosshair.SetActive(true); // Показываем прицел
        }
    }

    void StopAiming()
    {
        isAiming = false;
        camera.SetAiming(false);
        if (crosshair != null)
        {
            crosshair.SetActive(false); // Скрываем прицел
        }
    }

    void UpdateCameraPosition()
    {
        if (cameraRig == null || aimPosition == null)
        {
            Debug.LogWarning("CameraRig или AimPosition не назначены.");
            return;
        }

        if (isAiming)
        {
            // Плавное перемещение камеры к точке AimPosition
            cameraRig.position = Vector3.Lerp(cameraRig.position, aimPosition.position, Time.deltaTime * aimSpeed);
            cameraRig.rotation = Quaternion.Lerp(cameraRig.rotation, aimPosition.rotation, Time.deltaTime * aimSpeed);
        }
    }
}
