using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerEnemy : EnemyBase
{
    protected override Vector2Int GetAttackPosition(){
        int playerPosX = Player.Instance.currCharacter.PosX;
        int playerPosY = Player.Instance.currCharacter.PosY;

        System.Random rnd = new System.Random();

        int x = rnd.Next(playerPosX-1, playerPosX+2);
        int y = rnd.Next(playerPosY-1, playerPosY+2);

        return new Vector2Int(x,y);
    }

    public override float Attack(){
        int ind = Random.Range(0,CombatManager.Instance.EnemyList.Count - 1);
        float animationTime;

        ((EnemyBase)CombatManager.Instance.EnemyList[ind]).GetHealed(baseDamage);
        animationTime = gameManagerScript.GetSpellByName(spells[0]).PlayAnimation(((EnemyBase)CombatManager.Instance.EnemyList[ind]).PosX, ((EnemyBase)CombatManager.Instance.EnemyList[ind]).PosY);

        /*
        if(combPart[ind] is Character){
            ((EnemyBase)combPart[ind + 1]).GetHealed(baseDamage);

            animationTime = gameManagerScript.GetSpellByName(spells[0]).PlayAnimation(((EnemyBase)combPart[ind + 1]).PosX, ((EnemyBase)combPart[ind + 1]).PosY);
        }
        else{
            ((EnemyBase)combPart[ind]).GetHealed(baseDamage);
            animationTime = gameManagerScript.GetSpellByName(spells[0]).PlayAnimation(((EnemyBase)combPart[ind]).PosX, ((EnemyBase)combPart[ind]).PosY);
        }
        */

        this.Lowlight();
        return animationTime;
    }
}
