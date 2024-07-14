using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class Move : SpellBase
{

    public override List<Vector2Int> GetEffectedTiles(int posX, int posY){

        List<Vector2Int> coords = new List<Vector2Int>();
        int moveRange = Player.Instance.currCharacter.moveRange;
        int offset = moveRange;

        for(int i = -offset; i < offset; i++){
            if(math.abs(posX - (posX + i)) +  math.abs(posY - (posY + i))< moveRange ){
                coords.Add(new Vector2Int(posX+i, posY+i));
            }
        }

        Debug.Log(coords[2]);

        return coords;
    }

}
