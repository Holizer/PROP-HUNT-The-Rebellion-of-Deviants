using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;

public class HiderModelManager : MonoBehaviourPun
{
    [Header("������� ������� ������")]
    [SerializeField] private GameObject currentModel;

    public void AssignRandomNPCModel(List<GameObject> npcModels)
    {
        if (npcModels == null || npcModels.Count == 0)
        {
            Debug.LogError("��� ��������� NPC ������� ��� Hider!");
            return;
        }

        int randomIndex = Random.Range(0, npcModels.Count);
        GameObject selectedModel = npcModels[randomIndex];

        if (currentModel != null)
        {
            Destroy(currentModel);
        }

        // ������������� ������
        currentModel = Instantiate(selectedModel, transform.position, transform.rotation, transform);
        currentModel.name = selectedModel.name;

        // ��������� �������� (���� ����)
        PhotonAnimatorView photonAnimationView = currentModel.GetComponent<PhotonAnimatorView>();
        if (photonAnimationView != null)
        {
            photonAnimationView.enabled = true;
            photonView.ObservedComponents.Add(photonAnimationView);
        }

        // ����� RPC ��� ������������� ������ �� ������ ��������
        photonView.RPC("SyncModelOnAllClients", RpcTarget.Others, selectedModel.name);
    }

    [PunRPC]
    private void SyncModelOnAllClients(string modelName)
    {
        GameObject selectedModel = Resources.Load<GameObject>(modelName);

        if (selectedModel != null)
        {
            if (currentModel == null)
            {
                currentModel = Instantiate(selectedModel, transform.position, transform.rotation, transform);
                currentModel.name = selectedModel.name;

                PhotonAnimatorView photonAnimationView = currentModel.GetComponentInChildren<PhotonAnimatorView>();
                if (photonAnimationView != null)
                {
                    photonAnimationView.enabled = true;
                    photonView.ObservedComponents.Add(photonAnimationView);
                }

                Debug.Log($"Hider ������� ������ NPC: {currentModel.name}");
            }
            else
            {
                Debug.LogWarning("������ ��� �����������!");
            }
        }
        else
        {
            Debug.LogError("������ �� ������� �� �����: " + modelName);
        }
    }
}
