using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterAimPositionController : MonoBehaviour
{
    public Transform player;
    public Transform cameraTransform;
    public Transform hunterAimPosition;
    public float orbitRadius = 3f; 
    public Vector3 offset = new Vector3(0f, 1.5f, 0f);

    void Update()
    {
        // Рассчитываем направление от игрока к целевой точке
        Vector3 directionToAim = (cameraTransform.forward).normalized;

        // Применяем смещение и радиус орбиты
        hunterAimPosition.position = player.position + offset + directionToAim * orbitRadius;

        // Опционально: вращаем HunterAimPosition в сторону камеры для корректного ориентира
        hunterAimPosition.LookAt(cameraTransform.position);
    }
}
