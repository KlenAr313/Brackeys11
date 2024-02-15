using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int posX;
    [SerializeField] private int posY;

    [SerializeField] private int health;
    [SerializeField] private int baseDamage;
    [SerializeField] private int speed;
    [SerializeField] private List<string> yourSpells;
    private GameManager gameManagerScript;

    public string selectedSpell;

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

    void Start(){
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        selectedSpell = "Fireball";
        gameManagerScript.RefreshCurrentSpell();
    }

    void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            selectedSpell = yourSpells[0];
            gameManagerScript.RefreshCurrentSpell();

        }
        if (Input.GetKeyDown("2"))
        {
            selectedSpell = yourSpells[1];
            gameManagerScript.RefreshCurrentSpell();
        }
        if (Input.GetKeyDown("3"))
        {
            selectedSpell = yourSpells[2];
            gameManagerScript.RefreshCurrentSpell();
        }
    }
    
    public List<string> playerSpells(){
        return yourSpells;
    }

    public int GetFinalDamage(){
        return baseDamage * gameManagerScript.GetSpellDamage(selectedSpell);
    }
}
