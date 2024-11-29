using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;

public class HiderModelManager : MonoBehaviourPun
{
    [Header("Текущая игровая модель")]
    [SerializeField] private GameObject currentModel;

    public void AssignRandomNPCModel(List<GameObject> npcModels)
    {
        if (npcModels == null || npcModels.Count == 0)
        {
            Debug.LogError("Нет доступных NPC моделей для Hider!");
            return;
        }

        int randomIndex = Random.Range(0, npcModels.Count);
        GameObject selectedModel = npcModels[randomIndex];

        if (currentModel != null)
        {
            Destroy(currentModel);
        }

        Transform model = transform.Find("Model");
        if (model == null)
        {
            Debug.LogError("Не найден конетйнер Model!");
            return;
        }
        currentModel = Instantiate(selectedModel, model.position, model.rotation, model);
        currentModel.name = selectedModel.name;

        PhotonAnimatorView photonAnimationView = currentModel.GetComponent<PhotonAnimatorView>();
        if (photonAnimationView != null)
        {
            photonAnimationView.enabled = true;
            photonView.ObservedComponents.Add(photonAnimationView);
        }

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
                Transform model = transform.Find("Model");
                if (model == null)
                {
                    Debug.LogError("Не найден конетйнер Model!");
                    return;
                }
                currentModel = Instantiate(selectedModel, model.position, model.rotation, model);
                currentModel.name = selectedModel.name;

                PhotonAnimatorView photonAnimationView = currentModel.GetComponentInChildren<PhotonAnimatorView>();
                if (photonAnimationView != null)
                {
                    photonAnimationView.enabled = true;
                    photonView.ObservedComponents.Add(photonAnimationView);
                }

                Debug.Log($"Hider получил модель NPC: {currentModel.name}");
            }
            else
            {
                Debug.LogWarning("Модель уже установлена!");
            }
        }
        else
        {
            Debug.LogError("Модель не найдена по имени: " + modelName);
        }
    }
}
