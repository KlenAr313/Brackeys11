using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, IFighter
{
    private GameManager gameManagerScript;
    [SerializeField] private int posX;
    [SerializeField] private int posY;

    [SerializeField] private int health;
    [SerializeField] private int baseDamage;
    [SerializeField] private int setSpeed;

    public int PosX { get => posX; set => posX = value; }
    public int PosY { get => posY; set => posY = value; }
    public int Health { get => health; set => health = value; }
    public int BaseDamage { get => baseDamage; set => baseDamage = value; }
    public int Speed { get => setSpeed; set => setSpeed = value; }

    void Start(){
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }
    public virtual void Die(){
        this.gameObject.SetActive(false);
    }

    public virtual void GetDamaged(int amount){
        this.Health -= amount;
        if(this.Health <= 0){
            Die();
        }
    }

    public virtual void Move(int x, int y){
        if(Health >= 0){
            posX += x;
            posY += y;
        }
    }

    public virtual void Strike(){
       gameManagerScript.EnemyStrikes(new List<Vector2> {new Vector2(PosX-1,PosY)}, baseDamage); 
    }
}
