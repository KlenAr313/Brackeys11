using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Character : MonoBehaviour, IFighter
{
    [SerializeField] public int characterIndex;
    [SerializeField] private int posX;
    [SerializeField] private int posY;

    [SerializeField] public int health;
    private int baseHealth;
    [SerializeField] private int baseDamage;
    [SerializeField] public int mana;
    private int baseMana;
    [SerializeField] private int setSpeed;
    [SerializeField] public List<string> spells;
    [SerializeField] protected Sprite previewImage;

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

        //EZ itt mi???
        if (Input.GetKeyDown(KeyCode.R) && Player.Instance.characterScritps[0].health <= 0 
                    && Player.Instance.characterScritps[1].health <= 0 
                    && Player.Instance.characterScritps[2].health <= 0
                )
            {
                GameManager.Instance.GameOver();
            }
        //this.gameObject.SetActive(false);
    }

    public void GetDamaged(int amount, float waitTilDisappear){
        this.Health -= amount;
        UpdateStatUI.Invoke();
        if(this.Health <= 0){
            StartCoroutine(Die(waitTilDisappear));
        }
    }

    public void GetHealed(int amount){
        this.health = health + amount;
        if(baseHealth < health){
            health = baseHealth;
        }
        UpdateStatUI.Invoke();
    }

    public void DecreaseMana(int amount){
        mana -= amount;
        UpdateStatUI.Invoke();
        //Debug.Log("Mana levonva. Maradék: " + mana);
    }

    void OnValidate(){
        selectedSpell = spells[0];
        this.baseHealth = this.health;
        this.baseMana = this.mana;

        this.posX = (int)transform.position.x;
        this.posY = (int)transform.position.y;
    }

    void Start(){
    }

    void Update()
    {
        if(Player.Instance.currCharacter == this){

            //Ki ne hozd a Refresh-t kivülre mert meghalsz (csak akkor update-elődjön ha van változás, különben minden frame-en hivná)
            if (Input.GetKeyDown("1"))
            {
                selectedSpell = spells[0];
                GameManager.Instance.RefreshCurrentSpell();
            }
            if (Input.GetKeyDown("2"))
            {
                selectedSpell = spells[1];
                GameManager.Instance.RefreshCurrentSpell();
            }
            if (Input.GetKeyDown("3"))
            {
                selectedSpell = spells[2];
                GameManager.Instance.RefreshCurrentSpell();
            }
            if (Input.GetKeyDown("4"))
            {
                selectedSpell = spells[3];
                GameManager.Instance.RefreshCurrentSpell();
            }
            if (Input.GetKeyDown("5"))
            {
                selectedSpell = spells[4];
                GameManager.Instance.RefreshCurrentSpell();
            }
        }
    }

    public void GiveMana(int amount){
        mana += amount;
        if(mana > baseMana){
            mana = baseMana;
        }
        UpdateStatUI.Invoke();
    }

    public int GetBaseDamage(){
        return baseDamage;
    }
    
    public List<string> GetSpells(){
        return spells;
    }

    public int GetFinalDamage(){
        return baseDamage * GameManager.Instance.GetSpellDamage(selectedSpell);
    }

    public float Attack()
    {
        return 1;
    }

    public int GetBaseHealth(){
        return baseHealth;
    }

    public int GetBaseMana(){
        return baseMana;
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

    public void UpdateUI(){
        UpdateStatUI.Invoke();
    }

}
