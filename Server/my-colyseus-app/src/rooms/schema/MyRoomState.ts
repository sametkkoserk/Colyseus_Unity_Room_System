import { Schema, MapSchema, type } from "@colyseus/schema";


export class Player extends Schema {
  @type("string") id: string;
  @type("string") sessionId: string;
  @type("string") userName: string="player";
  @type("number") xPos: number = 0;
  @type("number") yPos: number = 0;
  @type("number") zPos: number = 0;
  @type("number") xRot: number = 0;
  @type("number") yRot: number = 0;
  @type("number") zRot: number = 0;
  @type("number") wRot: number = 0;
}
export class MyRoomState extends Schema {

  @type({ map: Player }) players = new MapSchema<Player>();

}
