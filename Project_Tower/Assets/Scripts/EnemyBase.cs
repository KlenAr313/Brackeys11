using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField] private int posX;
    [SerializeField] private int posY;

    [SerializeField] private int health;
    [SerializeField] private int baseDamage;
    [SerializeField] private int speed;

    public int PosX { get => posX; set => posX = value; }
    public int PosY { get => posY; set => posY = value; }
    public int Health { get => health; set => health = value; }
    public int BaseDamage { get => baseDamage; set => baseDamage = value; }
    public int Speed { get => speed; set => speed = value; }

    public void Die(){
        this.gameObject.SetActive(false);
    }

    public void GetDamaged(int amount){
        this.Health -= amount;
        if(this.Health <= 0){
            Die();
        }
    }

}
