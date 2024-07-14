using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Character : MonoBehaviour, IFighter
{
    [SerializeField] public int characterIndex;
    [SerializeField][HideInInspector] public int followPointIndex;
    [SerializeField][HideInInspector] public float offsetLength;
    [SerializeField][HideInInspector] public int frameCount;

    [SerializeField] private int posX;
    [SerializeField] private int posY;

    [SerializeField] public int health;
    private int baseHealth;
    [SerializeField] private int baseDamage;
    [SerializeField] public int mana;
    private int baseMana;
    [SerializeField] private int setSpeed;
    [SerializeField] public int moveRange;
    [SerializeField] public List<string> spells;
    [SerializeField][HideInInspector] protected Sprite previewImage;

    private Rigidbody2D rb;
    public Animator animator;

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

        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();

        followPointIndex = 0;
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
            if (Input.GetKeyDown("6"))
            {
                selectedSpell = spells[5];
                GameManager.Instance.RefreshCurrentSpell();
            }
            if (Input.GetKeyDown("7"))
            {
                selectedSpell = spells[6];
                GameManager.Instance.RefreshCurrentSpell();
            }
            if (Input.GetKeyDown("8"))
            {
                selectedSpell = spells[7];
                GameManager.Instance.RefreshCurrentSpell();
            }
            if (Input.GetKeyDown("9"))
            {
                selectedSpell = spells[8];
                GameManager.Instance.RefreshCurrentSpell();
            }
        }
    }

    
    public void Move(float moveX, float moveY){
        Vector2 moveVector = new Vector2(moveX, moveY).normalized * Time.fixedDeltaTime * Player.Instance.velocity;

        animator.SetFloat("Horizontal", moveX);
        animator.SetFloat("Vertical", moveY);

        animator.SetFloat("Speed", new Vector2(moveX, moveY).normalized.magnitude);
                
        rb.velocity = moveVector;
    }

    //Move with custom velocity
    public void Move(float moveX, float moveY, float velocityMultiplier){
        float vel = Player.Instance.velocity * velocityMultiplier;

        Vector2 moveVector = new Vector2(moveX, moveY).normalized * Time.fixedDeltaTime * vel;

        animator.SetFloat("Horizontal", moveX);
        animator.SetFloat("Vertical", moveY);

        animator.SetFloat("Speed", new Vector2(moveX, moveY).normalized.magnitude);
            
        rb.velocity = moveVector;
    }


    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Character"){
            Physics2D.IgnoreCollision(this.gameObject.GetComponent<Collider2D>(), col.gameObject.GetComponent<Collider2D>());
        }
    }


    public float Distance(float targetX, float targetY){
        return new Vector2(targetX - this.gameObject.transform.position.x, targetY - this.gameObject.transform.position.y).magnitude;
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
