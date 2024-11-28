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

        // Инстанциируем модель
        currentModel = Instantiate(selectedModel, transform.position, transform.rotation, transform);
        currentModel.name = selectedModel.name;

        // Настройка анимации (если есть)
        PhotonAnimatorView photonAnimationView = currentModel.GetComponent<PhotonAnimatorView>();
        if (photonAnimationView != null)
        {
            photonAnimationView.enabled = true;
            photonView.ObservedComponents.Add(photonAnimationView);
        }

        // Вызов RPC для синхронизации модели на других клиентах
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
