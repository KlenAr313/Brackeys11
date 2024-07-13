using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : EnemyBase
{

    protected override Vector2Int GetAttackPosition(){
        int playerPosX = GameManager.Instance.GetRandomCharacter().PosX;
        int playerPosY = GameManager.Instance.GetRandomCharacter().PosY;

        System.Random rnd = new System.Random();

        int x = rnd.Next(playerPosX-1, playerPosX+2);
        int y = rnd.Next(playerPosY-1, playerPosY+2);

        return new Vector2Int(x,y);
    }
}
