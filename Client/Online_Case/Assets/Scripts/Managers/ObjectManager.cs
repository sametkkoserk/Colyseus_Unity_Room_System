using System.Collections;
using System.Collections.Generic;
using Colyseus;
using Colyseus.Schema;
using GameDevWare.Serialization;
using LucidSightTools;
using NativeWebSocket;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    
    public Transform characterContainer;

    public Dictionary<string, OtherCharacterController> characters = new Dictionary<string, OtherCharacterController>();
    public SelfCharacterController selfCharacterController;
    public void SelfCharacterCreate(Player player)
    {
        Addressables.InstantiateAsync("SelfCharacter", characterContainer).Completed+= (instantiateAsync) =>
        {
            SelfCharacterController characterController=instantiateAsync.Result.GetComponent<SelfCharacterController>();
            selfCharacterController=characterController;

        };
    }

    public void OtherCharacterCreate(string playerID, Player player)
    {
        Addressables.InstantiateAsync("OtherCharacter", characterContainer).Completed+= (instantiateAsync) =>
        {
            OtherCharacterController characterController=instantiateAsync.Result.GetComponent<OtherCharacterController>();
            characters.Add(playerID,characterController);
            characterController.setPlayer(player);

        };
    }

    public void OtherCharacterController(string playerID, Vector3 pos, Quaternion rot)
    {
        if (characters.ContainsKey(playerID))
        {
            characters[playerID].setPosRot(pos,rot);
        }
    }
    
    public void CharacterQuit(string playerID)
    {
        if (characters.ContainsKey(playerID))
        {
            characters[playerID].OnQuit();
        }
    }

    public void QuitRoom()
    {
        foreach (OtherCharacterController character in characters.Values)
        {
            Destroy(character.gameObject);
        }
        Destroy(selfCharacterController.gameObject);
        characters.Clear();
    }
}
