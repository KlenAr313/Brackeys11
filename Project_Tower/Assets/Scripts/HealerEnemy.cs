using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerEnemy : EnemyBase
{
    protected override Vector2Int GetAttackPosition(){
        int playerPosX = gameManagerScript.playerScript.PosX;
        int playerPosY = gameManagerScript.playerScript.PosY;

        System.Random rnd = new System.Random();

        int x = rnd.Next(playerPosX-1, playerPosX+2);
        int y = rnd.Next(playerPosY-1, playerPosY+2);

        return new Vector2Int(x,y);
    }

    public override float Attack(){
        List<IFighter> combPart = gameManagerScript.combatManagerScript.combatParticipants;
        int ind = Random.Range(0,combPart.Count - 1);
        float animationTime;
        if(combPart[ind] is Player){
            ((EnemyBase)combPart[ind + 1]).GetHealed(baseDamage);

            animationTime = gameManagerScript.GetSpellByName(spells[0]).PlayAnimation(((EnemyBase)combPart[ind + 1]).PosX, ((EnemyBase)combPart[ind + 1]).PosY);
        }
        else{
            ((EnemyBase)combPart[ind]).GetHealed(baseDamage);
            animationTime = gameManagerScript.GetSpellByName(spells[0]).PlayAnimation(((EnemyBase)combPart[ind]).PosX, ((EnemyBase)combPart[ind]).PosY);
        }

        this.Lowlight();
        return animationTime;
    }
}
