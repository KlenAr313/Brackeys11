using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour, IFighter
{
    public static Player Instance{get; private set;}

    [SerializeField] private int posX;
    [SerializeField] private int posY;

    [SerializeField] public int health;
    [SerializeField] private int baseDamage;
    [SerializeField] public int mana;
    [SerializeField] public int baseMana;
    [SerializeField] private int setSpeed;
    [SerializeField] public List<string> yourSpells;
    [SerializeField] protected Sprite previewImage;
    private GameManager gameManagerScript;
    private int baseHealth;

    public string selectedSpell;
    public event Action UpdateStatUI;

    public int PosX { get => posX; set => posX = value; }
    public int PosY { get => posY; set => posY = value; }
    public int Health { get => health; set => health = value; }
    public int BaseDamage { get => baseDamage; set => baseDamage = value; }
    public int Speed { get => setSpeed; set => setSpeed = value; }
    public Sprite PreviewImage { get => previewImage; set => previewImage = value; }

    public IEnumerator Die(float waitTilDisappear){
        yield return new WaitForSeconds(waitTilDisappear);
        gameManagerScript.GameOver();
        //this.gameObject.SetActive(false);
    }

    public void GetDamaged(int amount, float waitTilDisappear){
        this.Health -= amount;
        UpdateStatUI?.Invoke();
        if(this.Health <= 0){
            StartCoroutine(Die(waitTilDisappear));
        }
    }

    public void DecreaseMana(int amount){
        mana -= amount;
        UpdateStatUI?.Invoke();
        Debug.Log("Mana levonva. Maradék: " + mana);
    }

    void Start(){
        Instance = this;

        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        selectedSpell = "Fireball";
        gameManagerScript.RefreshCurrentSpell();
        this.baseHealth = this.health;

        this.posX = (int)transform.position.x;
        this.posY = (int)transform.position.y;
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

        if (Input.GetKeyDown(KeyCode.R) && health <= 0)
        {
            Debug.Log("Restart button pressed");
            gameManagerScript.Restart();
        }
    }

    public static int GetPlayerBaseDamage(){
        return Instance.baseDamage;
    }
    
    public List<string> playerSpells(){
        return yourSpells;
    }

    public int GetFinalDamage(){
        return baseDamage * gameManagerScript.GetSpellDamage(selectedSpell);
    }

    public float Attack()
    {
        return 1;
    }

    public int GetBaseHealth(){
        return baseHealth;
    }

    public void RefreshPosition(){
        this.posX = (int)this.transform.position.x;
        this.posY = (int)this.transform.position.y;
    }

    public void SetPosition(int newPosX, int newPosY){
        this.transform.position = new Vector3((float) newPosX, (float) newPosY);
        this.posX = newPosX;
        this.posY = newPosY;
    }

}
