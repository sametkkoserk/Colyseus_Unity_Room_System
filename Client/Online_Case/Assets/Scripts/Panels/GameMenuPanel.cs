using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Serialization;

public class GameMenuPanel : MonoBehaviour
{
    public void OnBackClick()
    {
        NetworkManager.instance.room.Leave();
        Addressables.InstantiateAsync("JoinOrCreateRoomPanel", transform.parent);
        Destroy(gameObject);
    }
}
