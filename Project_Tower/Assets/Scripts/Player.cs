using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour, IFighter
{
    [SerializeField] private int posX;
    [SerializeField] private int posY;

    [SerializeField] public int health;
    [SerializeField] public int baseHealth;
    [SerializeField] private int baseDamage;
    [SerializeField] public int mana;
    [SerializeField] public int baseMana;
    [SerializeField ]private int setSpeed;
    [SerializeField] private List<string> yourSpells;
    private GameManager gameManagerScript;

    public string selectedSpell;
    public event Action UpdateStatUI;

    public int PosX { get => posX; set => posX = value; }
    public int PosY { get => posY; set => posY = value; }
    public int Health { get => health; set => health = value; }
    public int BaseDamage { get => baseDamage; set => baseDamage = value; }
    public int Speed { get => setSpeed; set => setSpeed = value; }

    public void Die(){
        this.gameObject.SetActive(false);
    }

    public void GetDamaged(int amount){
        this.Health -= amount;
        UpdateStatUI?.Invoke();
        if(this.Health <= 0){
            Die();
        }
    }

    public void DecreaseMana(int amount){
        mana -= amount;
        UpdateStatUI?.Invoke();
        Debug.Log("Mana levonva. Maradék: " + mana);
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

    public void Attack()
    {
        
    }
}
