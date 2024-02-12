using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField] protected int posX;
    [SerializeField] protected int posY;

    [SerializeField] protected int health; 
    [SerializeField] protected int baseDamage;
    [SerializeField] protected int speed;

    private void setPosX( int posX){
        this.posX = posX;
    }

    private int getPosX( int posX){
        return posX;
    }

    private void setPosY( int posY){
        this.posY = posY;
    }

    private int getPosY( int posY){
        return posY;
    }


}
