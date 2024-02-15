using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    private GameManager gameManagerScript;
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

    void Start(){
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }
    public virtual void Die(){
        this.gameObject.SetActive(false);
        Debug.Log("I died");
    }

    public virtual void GetDamaged(int amount){
        Debug.Log("E "+Health);
        this.Health -= amount;
        if(this.Health <= 0){
            Die();
        }
        Debug.Log("U "+ Health);
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
