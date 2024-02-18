using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NukerEnemy : EnemyBase
{
    protected override Vector2Int GetAttackPosition()
    {
        System.Random rnd = new System.Random();
        List<Vector2Int> list = gameManagerScript.tileManagerScript.GetPlayableArea(1);
        
        return list[rnd.Next(0, list.Count)];
    }
}
