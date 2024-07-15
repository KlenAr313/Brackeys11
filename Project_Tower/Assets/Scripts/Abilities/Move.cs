using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Move : SpellBase
{

    public override List<Vector2Int> GetEffectedTiles(int posX, int posY){

        List<Vector2Int> coords = new List<Vector2Int>();
        posX = Player.Instance.currCharacter.PosX;
        posY = Player.Instance.currCharacter.PosY;
        int moveRange = Player.Instance.currCharacter.moveRange;

        for(int i = -moveRange; i <= moveRange; i++){
            
            for(int j = -moveRange; j <= moveRange; j++){

                if(math.abs(posX - (posX + i)) +  math.abs(posY - (posY + j)) <= moveRange ){
                    coords.Add(new Vector2Int(posX+i, posY+j));
                }

            }


            //Debug.Log("Távolság X: " + math.abs(posX - (posX + i)));
            //Debug.Log("Távolság Y: " + math.abs(posY - (posX + i)));

        }

        //Debug.Log(coords[2]);

        return coords;
    }

    public int GetMoveRangeCost(int targetX, int targetY){
        return math.abs(Player.Instance.currCharacter.PosX - targetX) + math.abs(Player.Instance.currCharacter.PosY - targetY);
    }

    public void PerformMove(int MoveToPosX, int MoveToPosY){
        if(GetEffectedTiles(Player.Instance.currCharacter.PosX, Player.Instance.currCharacter.PosX).Contains(new Vector2Int(MoveToPosX, MoveToPosY))){
            Player.Instance.currCharacter.transform.position = new Vector3(MoveToPosX, MoveToPosY, 0);
            Player.Instance.currCharacter.moveRange -= GetMoveRangeCost(MoveToPosX, MoveToPosY);
            Player.Instance.currCharacter.UpdatePosition();

            GameManager.Instance.TileHighlighter(MoveToPosX,MoveToPosY);
        }
    }

}
