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
        // ������������ ����������� �� ������ � ������� �����
        Vector3 directionToAim = (cameraTransform.forward).normalized;

        // ��������� �������� � ������ ������
        hunterAimPosition.position = player.position + offset + directionToAim * orbitRadius;

        // �����������: ������� HunterAimPosition � ������� ������ ��� ����������� ���������
        hunterAimPosition.LookAt(cameraTransform.position);
    }
}
