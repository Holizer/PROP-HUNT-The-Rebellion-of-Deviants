using Photon.Pun;
using UnityEngine;

public class HiderModelManager : MonoBehaviourPun
{
    [Header("Текущая игровая модель")]
    [SerializeField] private GameObject currentModel;

    public void AssignReservedNPCModel()
    {
        GameObject reservedModel = FindObjectOfType<NPCManager>().GetReservedNPCModel();
        if (reservedModel == null)
        {
            Debug.LogError("Зарезервированная модель NPC не найдена!");
            return;
        }

        if (currentModel != null)
        {
            Destroy(currentModel);
        }

        Transform modelContainer = transform.Find("Model");
        if (modelContainer == null)
        {
            Debug.LogError("Не найден контейнер Model!");
            return;
        }

        currentModel = Instantiate(reservedModel, modelContainer.position, modelContainer.rotation, modelContainer);
        currentModel.name = reservedModel.name;

        PhotonAnimatorView photonAnimationView = currentModel.GetComponent<PhotonAnimatorView>();
        if (photonAnimationView != null)
        {
            photonAnimationView.enabled = true;
            photonView.ObservedComponents.Add(photonAnimationView);
        }

        photonView.RPC("SyncModelOnAllClients", RpcTarget.Others, reservedModel.name);
    }

    [PunRPC]
    private void SyncModelOnAllClients(string modelName)
    {
        GameObject selectedModel = Resources.Load<GameObject>(modelName);
        if (selectedModel != null)
        {
            Transform modelContainer = transform.Find("Model");
            if (modelContainer == null)
            {
                Debug.LogError("Не найден контейнер Model!");
                return;
            }

            if (currentModel != null)
            {
                Destroy(currentModel);
            }

            currentModel = Instantiate(selectedModel, modelContainer.position, modelContainer.rotation, modelContainer);
            currentModel.name = selectedModel.name;

            PhotonAnimatorView photonAnimationView = currentModel.GetComponent<PhotonAnimatorView>();
            if (photonAnimationView != null)
            {
                photonAnimationView.enabled = true;
                photonView.ObservedComponents.Add(photonAnimationView);
            }

            Debug.Log($"Hider получил модель NPC: {currentModel.name}");
        }
        else
        {
            Debug.LogError("Модель не найдена по имени: " + modelName);
        }
    }
}