using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;

public class LocalPlayerValidator : MonoBehaviour
{
    [Tooltip("��������� ��������� ���������� ��� ������������ ������.")]
    public MonoBehaviour[] componentsToDisable;

    [Tooltip("��������� ��������� ������� ��� ������������ ������.")]
    public GameObject[] objectsToDisable;

    void Awake()
    {
        if (!TryGetComponent<PhotonView>(out var photonView) || !photonView.IsMine)
        {
            foreach (var component in componentsToDisable)
            {
                component.enabled = false;
            }

            foreach (var obj in objectsToDisable)
            {
                obj.SetActive(false);
            }

            enabled = false;
        }
    }
}