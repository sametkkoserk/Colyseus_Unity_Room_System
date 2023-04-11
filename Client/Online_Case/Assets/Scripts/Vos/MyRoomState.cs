using Colyseus.Schema;

public class MyRoomState : Schema
{
  [Type(0, "map", typeof(MapSchema<Player>))]
  public MapSchema<Player> players = new MapSchema<Player>();
}