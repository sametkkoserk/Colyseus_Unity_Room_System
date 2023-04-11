using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class OtherCharacterController : MonoBehaviour
{
  [FormerlySerializedAs("userNameTextBehaviour")]
  [FormerlySerializedAs("userNameText")]
  public UserNameTextController userNameTextController;
  private Player player;
  public void setPosRot(Vector3 pos, Quaternion rot)
  {
    transform.position = pos;
    transform.rotation = rot;
  }

  public void OnQuit()
  {
    Destroy(gameObject);
  }

  public void setPlayer(Player player)
  {
    this.player = player;
    userNameTextController.SetText(player.userName);
  }
}