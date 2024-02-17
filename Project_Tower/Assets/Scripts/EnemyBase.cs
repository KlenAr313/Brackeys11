using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, IFighter
{
    protected GameManager gameManagerScript;
    [SerializeField] protected int posX;
    [SerializeField] protected int posY;
    [SerializeField] protected List<string> spells;

    [SerializeField] protected int health;
    [SerializeField] protected int baseDamage;
    [SerializeField] protected int setSpeed;

    [SerializeField] protected Sprite previewImage;

    public int PosX { get => posX; set => posX = value; }
    public int PosY { get => posY; set => posY = value; }
    public int Health { get => health; set => health = value; }
    public int BaseDamage { get => baseDamage; set => baseDamage = value; }
    public int Speed { get => setSpeed; set => setSpeed = value; }
    public Sprite PreviewImage { get => previewImage; set => previewImage = value; }

    void Start(){
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        this.posX = (int)this.gameObject.transform.position.x;
        this.posY = (int)this.gameObject.transform.position.y;
    }
    public IEnumerator Die(float waitTilDisappear){
        yield return new WaitForSeconds(waitTilDisappear);
        this.gameObject.SetActive(false);
        Debug.Log("I died");
    }

    public virtual void GetDamaged(int amount, float waitTilDisappear){
        this.Health -= amount;
        if(this.Health <= 0){
            StartCoroutine(Die(waitTilDisappear));
        }
        Debug.Log("U "+ Health);
    }

    public virtual void Move(int x, int y){
        if(Health >= 0){
            posX += x;
            posY += y;
        }
    }

    public virtual float Attack(){
        //Ide lehet irni a spell kiválasztás logikáját
        Vector2Int target = this.GetAttackPosition();
        float animationTime = gameManagerScript.GetSpellByName(spells[0]).PlayAnimation(target.x, target.y);
        foreach(Vector2Int coord in gameManagerScript.GetSpellByName(spells[0]).Cast(target.x, target.y)){
            if(coord.x == gameManagerScript.playerScript.PosX && coord.y == gameManagerScript.playerScript.PosY){
                gameManagerScript.playerScript.GetDamaged(baseDamage, animationTime);
                Debug.Log("Player damaged");
            }
        }
        return animationTime;
    }

    protected abstract Vector2Int GetAttackPosition();

}
