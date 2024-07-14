using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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

                if(math.abs(posX - (posX + i)) +  math.abs(posY - (posY + j))< moveRange ){
                    coords.Add(new Vector2Int(posX+i, posY+j));
                }

            }


            Debug.Log("Távolság X: " + math.abs(posX - (posX + i)));
            Debug.Log("Távolság Y: " + math.abs(posY - (posX + i)));

        }

        //Debug.Log(coords[2]);

        return coords;
    }

}
