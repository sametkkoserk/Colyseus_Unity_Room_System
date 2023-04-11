using System.Collections;
using System.Collections.Generic;
using Colyseus;
using Colyseus.Schema;
using GameDevWare.Serialization;
using LucidSightTools;
using NativeWebSocket;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class NetworkManager : MonoBehaviour
{
    public static  NetworkManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private IndexedDictionary<string, Player> users =
        new IndexedDictionary<string, Player>();
    public ColyseusClient client;
    public ColyseusRoom<MyRoomState> room;
    public string currentUserName;
    public Player currentPlayer;
    void Start()
    {
        client = new ColyseusClient("ws://localhost:2567");
    }
    public async void JoinOrCreateRoom(string userName)
    {
        Dictionary<string, object> playerInfo = new Dictionary<string, object>();
        playerInfo["playerInfo"] = new PlayerInfoVo() { userName = userName };
        room = await client.JoinOrCreate<MyRoomState>("my_room",playerInfo);
        currentUserName = userName;
        
        room.OnMessage<Player>("joined",player=>
        {
            Debug.Log("Joined to the room");
            currentPlayer = player;
        });
        OnJoinRoom();

    }
    public async void JoinRoom(string userName)
    {
        room = await client.Join<MyRoomState>("my_room");
        OnJoinRoom();
    }
    public void OnJoinRoom()
    {
        room.OnLeave += OnLeaveRoom;

        // Something has been added to Schema
        room.OnStateChange+= OnStateChangeHandler;

        room.State.players.OnAdd += OnUserAdd;
        // Something has changed in Schema
        room.State.players.OnChange += OnUserChange;

        // Something has been removed from Schema
        room.State.players.OnRemove += OnUserRemove;
    }

    private void OnUserChange(string key, Player player)
    {
        Debug.Log($"user [ {player.id} | {player.xPos} |{player.yPos} |{player.zPos} |{player.xRot} |{player.yRot} |key {key}] changed");
    }
    private void OnUserAdd(string key, Player player)
    {
        Debug.Log($"user [  {player.__refId} | {player.id} | key {key}] Joined");
        users.Add(key, player);
        if (currentUserName==player.userName)
        {
            ObjectManager.instance.SelfCharacterCreate(player);
        }
        else
        {
            ObjectManager.instance.OtherCharacterCreate(player.id,player);
        }
    }
    private void OnUserRemove(string key, Player player)
    {
        Debug.Log($"user [ {player.id} | key {key}] removed");
        users.Remove(key);   
        ObjectManager.instance.CharacterQuit(player.id);
        room.State.players.OnAdd -= OnUserAdd;
        room.State.players.OnChange -= OnUserChange;
        room.State.players.OnRemove -= OnUserRemove;
    }

    private void OnStateChangeHandler(MyRoomState state, bool isFirstState)
    {
        // Do something with the state
        Debug.Log("state change kanka");
        room.State.players.ForEach((string key, Player player) =>
        {
            Debug.Log(" Value: " + player);

            if (currentPlayer.userName != player.userName)
            {
                users[key]=player;
                Vector3 pos = new Vector3(player.xPos, player.yPos, player.zPos);
                Quaternion rot = new Quaternion(player.xRot, player.yRot, player.zRot, player.wRot);
                ObjectManager.instance.OtherCharacterController(player.id,pos,rot);
                Debug.Log("Key: " + key + ", Value: " + player);
            }
            
        });
    }
    public void OnLeaveRoom(int code)
    {
        currentPlayer = null;
        currentUserName = "";
        users.Clear();
        ObjectManager.instance.QuitRoom();
        
        WebSocketCloseCode closeCode = WebSocketHelpers.ParseCloseCodeEnum(code);
        LSLog.Log(string.Format("ROOM: ON LEAVE =- Reason: {0} ({1})", closeCode, code));
    }
    
    
    void OnDestroy()
    {
#if UNITY_EDITOR
        OnApplicationQuit();
#endif
    }
    void OnApplicationQuit()
    {
        room.Leave();
    }

    public void SendPosRot(Vector3 position, Quaternion rotation)
    {
        PosRotVo vo = new PosRotVo()
        {
            xPos = position.x,
            yPos = position.y,
            zPos = position.z,

            xRot = rotation.x,
            yRot = rotation.y,
            zRot = rotation.z,
            wRot = rotation.w
        };
        room.Send("PosRot", vo);
    }
}

