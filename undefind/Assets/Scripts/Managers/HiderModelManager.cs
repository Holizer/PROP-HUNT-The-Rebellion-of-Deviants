using Photon.Pun;
using UnityEngine;

public class HiderModelManager : MonoBehaviourPun
{
    [Header("������� ������� ������")]
    [SerializeField] private GameObject currentModel;

    public void AssignReservedNPCModel()
    {
        GameObject reservedModel = FindObjectOfType<NPCManager>().GetReservedNPCModel();
        if (reservedModel == null)
        {
            Debug.LogError("����������������� ������ NPC �� �������!");
            return;
        }

        if (currentModel != null)
        {
            Destroy(currentModel);
        }

        Transform modelContainer = transform.Find("Model");
        if (modelContainer == null)
        {
            Debug.LogError("�� ������ ��������� Model!");
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
                Debug.LogError("�� ������ ��������� Model!");
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

            Debug.Log($"Hider ������� ������ NPC: {currentModel.name}");
        }
        else
        {
            Debug.LogError("������ �� ������� �� �����: " + modelName);
        }
    }
}