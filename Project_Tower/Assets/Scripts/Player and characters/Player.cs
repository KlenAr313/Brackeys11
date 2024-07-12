using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    public bool canMove;
    public float velocity;
    public Animator animator;

    public Character currCharacter;

    [SerializeField] public SpellBase currentSpell;
    [SerializeField] public List<Character> characterScritps;

    private Rigidbody2D rb;
    void OnValidate()
    {
        if(Instance == null){
            Instance = this;
        }

        if(Instance != this){
            Destroy(this.gameObject);
        }

        characterScritps.Clear();
        

        foreach(Transform child in this.transform){
            if(child.TryGetComponent<Character>(out Character characterScript)){
                characterScritps.Add(characterScript);
            }
        }

        canMove = true;

        rb = transform.GetChild(0).GetComponent<Rigidbody2D>();
        animator = transform.GetChild(0).GetComponent<Animator>();

        currCharacter = characterScritps[0];
        if(GameManager.Instance != null){
            currentSpell = GameManager.Instance.GetSpellByName(currCharacter.GetSpells()[0]);
            GameManager.Instance.RefreshCurrentSpell();
        }
    }

    void Awake(){
        DontDestroyOnLoad(this.gameObject);
        currentSpell = GameManager.Instance.GetSpellByName(currCharacter.GetSpells()[0]);
    }

    void Update(){
            if (Input.GetKeyDown(KeyCode.R) 
                    && characterScritps[0].health <= 0 
                    && characterScritps[1].health <= 0 
                    && characterScritps[2].health <= 0
                )
            {
                GameManager.Instance.Restart();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }

            if(canMove){
                float moveX = Input.GetAxisRaw("Horizontal");
                float moveY = Input.GetAxisRaw("Vertical");

                Vector2 moveVector = new Vector2(moveX, moveY).normalized * Time.deltaTime * velocity;
                animator.SetFloat("Horizontal", moveX);
                animator.SetFloat("Vertical", moveY);
                animator.SetFloat("Speed", new Vector2(moveX, moveY).normalized.magnitude);
                
                rb.velocity = moveVector;
            }
    }

    public void EnableFreeMovement(){
        canMove = true;
    }

    public void DisableFreeMovement(){
        canMove = false;
    }

    public PlayerSaveData GetSaveInfo(){

        List<int> characterHealths = new List<int>();
        List<int> characterManas = new List<int>();;

        foreach(Character character in characterScritps){
            characterHealths.Add(character.health);
            characterManas.Add(character.mana);
        }

        return new PlayerSaveData(characterHealths, characterManas);
    }

    public void LoadFromData(PlayerSaveData data){

        //Esetleges problémák a karakter object-ek loadolásával
        for(int i = 0; i < characterScritps.Count; i++){
            characterScritps[i].health = data.characterHealths[i];
            characterScritps[i].mana = data.characterManas[i];
        }
    }

}
