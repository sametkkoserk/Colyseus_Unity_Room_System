import { Room, Client } from "colyseus";
import { MyRoomState, Player } from "./schema/MyRoomState";
import { Enums } from "./schema/Enums";

export class MyRoom extends Room<MyRoomState> {

  onCreate (options: any) {
    this.setState(new MyRoomState());
    console.log(this.roomId, "created!");

    this.onMessage("type", (client, message) => {
      console.log(message)
    });

  }

  onJoin (client: Client, options: any) {
    console.log(client.sessionId, "joined!");
    var playerInfo = options.playerInfo;
    var userName = playerInfo.userName;
    console.log(userName);
    let player=new Player().assign({
      id: client.id,
      sessionId: client.sessionId,
      userName: userName,
    });

    client.send("joined",player);
    this.state.players.set(client.sessionId,player);
    this.onMessage("PosRot", (client, message) => {
      console.log(message);
      var player=this.state.players.get(client.sessionId);
      player.xPos=message.xPos
      player.yPos=message.yPos
      player.zPos=message.zPos

      player.xRot=message.xRot
      player.yRot=message.yRot
      player.zRot=message.zRot
      player.wRot=message.wRot
    });
  }

  onLeave (client: Client, consented: boolean) {
    console.log(client.sessionId, "left!");
    this.state.players.delete(client.sessionId);
  }

  onDispose() {
    console.log("room", this.roomId, "disposing...");
  }

}
