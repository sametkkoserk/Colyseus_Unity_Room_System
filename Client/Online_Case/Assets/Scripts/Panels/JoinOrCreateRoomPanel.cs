using System.Collections;
using System.Collections.Generic;
using Colyseus;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class JoinOrCreateRoomPanel : MonoBehaviour
{
    public TMP_InputField userName;
    public void OnCreate()
    {
        NetworkManager.instance.JoinOrCreateRoom(userName.text);
        Addressables.InstantiateAsync("GameMenuPanel", transform.parent);
        Destroy(gameObject);
    }

}
