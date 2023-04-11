using TMPro;
using UnityEngine;

public class UserNameTextController : MonoBehaviour
{
  public void SetText(string userName)
  {
    GetComponent<TMP_Text>().text = userName;
  }

}