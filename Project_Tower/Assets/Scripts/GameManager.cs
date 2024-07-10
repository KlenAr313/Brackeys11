using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //0 mindig a player
    [SerializeField] public bool isPlayerTurn;
    [SerializeField] public bool isFighting;
    [SerializeField] public GameObject playerObj;
    [SerializeField] public RoomManager roomManagerScript;
    [SerializeField] public List<Character> characterScritps;
    [SerializeField] public TileManager tileManagerScript;
    [SerializeField] public List<SpellBase> spellList;
    [SerializeField] public SpellBase currentSpell;
    [SerializeField] public CombatManager combatManagerScript;

    //Basically the length of the spell bar
    [SerializeField] public static int MaxAbilities = 3;

    public bool canClick = true;
    private int currentX;
    private int currentY;
    public Character currCharacter;

    public event Action SpellRefreshed;
    public event Action OnGameOver;

    public static GameManager Instance;

    void Awake(){

        if(Instance == null){
            DontDestroyOnLoad(this.gameObject);
            Instance = this;
        }
        
        if(Instance != this){
            Destroy(this.gameObject);
        }

        //Debug miatt true, false legyen alapból!
#if DEBUG
        //isFighting = true;
#else
        isFighting = false;
#endif
    }

    void OnValidate(){
        characterScritps.Clear();
        spellList.Clear();  
        roomManagerScript = GameObject.Find("Room Manager").GetComponent<RoomManager>();
        tileManagerScript = GameObject.Find("Tile Manager").GetComponent<TileManager>();
        combatManagerScript = gameObject.GetComponent<CombatManager>();

        playerObj = GameObject.Find("Player").gameObject;
        foreach(Transform child in playerObj.transform){
            if(child.TryGetComponent<Character>(out Character characterScript)){
                characterScritps.Add(characterScript);
            }
        }

        GameObject spellsObj = this.gameObject.transform.Find("Spells").gameObject;
        Component[] components = spellsObj.GetComponents(typeof(Component));
        foreach(Component comp in components){
            if(comp.ToString() != "Spells (UnityEngine.Transform)"){
                spellList.Add((SpellBase)comp);
            }
        }

        currentX = -1;
        currentY = -1;

        currCharacter = characterScritps[0];
        currentSpell = GetSpellByName(currCharacter.GetSpells()[0]);

        isPlayerTurn = false;
        RefreshCurrentSpell();
    }

    public void TileClicked(int posX, int posY){

        if(!canClick){
            return;
        }

        if(tileManagerScript.IsTileClickable(posX, posY)){
            currentX = posX;
            currentY = posY;
        }

        if(isFighting){
            //Player köre
            if(isPlayerTurn){
                //currCharacter = characterScritps[currCharacterIndex];
                if(currentSpell != null && currentSpell.ManaCost <= currCharacter.mana){
                    foreach(Vector2Int coord in currentSpell.Cast(currentX, currentY)){
                        //Debug.Log("Tile effected by " + currentSpell.spellName + ": X: " + coord.x + " Y: " + coord.y);
                        roomManagerScript.TileClicked(coord.x, coord.y, true);
                    }

                    currentSpell.PlayAnimation(currentX, currentY);
                    currentSpell.PlaySound();
                    tileManagerScript.RemoveAllHighlight();


                    currCharacter.DecreaseMana(currentSpell.ManaCost);
                    StartCoroutine(combatManagerScript.PlayerTakeTurn());
                }
            }
        }
        else
        {
            roomManagerScript.TileClicked(posX, posY, false);
        }
    }

    public void TileHighlighter(int posX, int posY){

        if(!isPlayerTurn){
            return;
        }

        if(!canClick){
            return;
        }

        if(tileManagerScript.IsTileClickable(posX, posY)){
            currentX = posX;
            currentY = posY;
        }
        if(isFighting){
            //Player köre
            if(isPlayerTurn){
                if(currentSpell != null){
                    foreach(Vector2Int coord in currentSpell.Cast(currentX, currentY)){
                        tileManagerScript.highlightSpellPreview(coord.x, coord.y);
                    }
                }
            }
        }
    }

    public void StartFight(){
        this.isFighting = true;
        combatManagerScript.StartCombat();
    }

    public void EndFight(){
        this.isFighting = false;
        roomManagerScript.WinFight();
    }

    public void RefreshCurrentSpell(){
        foreach(SpellBase spell in spellList){
            if(spell.spellName == currCharacter.selectedSpell){
                currentSpell = spell;
            }
        }

        if(currentX != -1 && currentY != -1){
            if(tileManagerScript != null){
                tileManagerScript.RemoveAllHighlight();
            }
            TileHighlighter(currentX, currentY);
        }

        SpellRefreshed?.Invoke();
    }

    public SpellBase GetSpellByName(string spellName){
        foreach(SpellBase spell in spellList){
            if(spell.spellName == spellName){
                return spell;
            }
        }
        return null;
    }

    public int GetSpellDamage(string spellName){
        foreach(SpellBase spell in spellList){
            if(spell.spellName == spellName){
                return spell.DamageModifier;
            }
        }
        return 0;
    }

    public void GameOver(){
        OnGameOver.Invoke();
    }

    public void Restart(){
        //Végleges Scene név re beirni
        SceneManager.LoadScene(0);
    }

    public Character GetRandomCharacter(){
        System.Random rnd = new System.Random();
        return characterScritps[rnd.Next(0,characterScritps.Count)];
    }


//Debug buttons
#if DEBUG
    void Update(){
        if(Input.GetKeyDown(KeyCode.K)){
            Debug.Log("Combat started!");
            isFighting = true;
            combatManagerScript.StartCombat();
        }

        if(Input.GetKeyDown(KeyCode.L)){
            Debug.Log("Combat ended!");
            isFighting = false;
            tileManagerScript.RemoveAllHighlight();
            combatManagerScript.EndCombat();
        }

        if(Input.GetKeyDown(KeyCode.P)){
            Debug.Log("P gombnyomás");
            tileManagerScript.GetPlayableArea(1);
        }
    } 
#endif

}
