using Colyseus.Schema;

[System.Serializable]
public class Player : Schema
{

  [Type(0, "string")]
  public string id = default(string);

  [Type(1, "string")]
  public string sessionId = default(string);
  
  [Type(2, "string")]
  public string userName = default(string);
  
  [Type(3, "number")]
  public float xPos = default(float);

  [Type(4, "number")]
  public float yPos = default(float);

  [Type(5, "number")]
  public float zPos = default(float);

  [Type(6, "number")]
  public float xRot = default(float);

  [Type(7, "number")]
  public float yRot = default(float);

  [Type(8, "number")]
  public float zRot = default(float);

  [Type(9, "number")]
  public float wRot = default(float);
}