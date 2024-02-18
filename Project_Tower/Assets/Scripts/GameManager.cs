using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //0 mindig a player
    [SerializeField] public bool isPlayerTurn;
    [SerializeField] public bool isFighting;
    [SerializeField] public GameObject playerObj;
    [SerializeField] public Player playerScript;
    [SerializeField] public RoomManager roomManagerScript;
    [SerializeField] public TileManager tileManagerScript;
    [SerializeField] public List<SpellBase> spellList;
    [SerializeField] public SpellBase currentSpell = null;
    [SerializeField] public CombatManager combatManagerScript;

    //Basically the length of the spell bar
    [SerializeField] public static int MaxAbilities = 3;

    public bool canClick = true;
    private int currentX;
    private int currentY;

    public event Action SpellRefreshed;
    public event Action OnGameOver;

    void Awake(){
        playerObj = GameObject.Find("Player").gameObject;
        playerScript = GameObject.Find("Player").GetComponent<Player>();
        roomManagerScript = GameObject.Find("Room Manager").GetComponent<RoomManager>();
        tileManagerScript = GameObject.Find("Tile Manager").GetComponent<TileManager>();
        combatManagerScript = gameObject.GetComponent<CombatManager>();

        GameObject spellsObj = this.gameObject.transform.Find("Spells").gameObject;

        currentX = -1;
        currentY = -1;

        Component[] components = spellsObj.GetComponents(typeof(Component));
        foreach(Component comp in components){
            if(comp.ToString() != "Spells (UnityEngine.Transform)"){
                spellList.Add((SpellBase)comp);
                Debug.Log("Spell Added");
            }
        }

        isPlayerTurn = false;

        RefreshCurrentSpell();

        //Debug miatt true, false legyen alapból!
#if DEBUG
        //isFighting = true;
#else
        isFighting = false;
#endif
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
                if(currentSpell != null && currentSpell.ManaCost <= playerScript.mana){
                    foreach(Vector2Int coord in currentSpell.Cast(currentX, currentY)){
                        //Debug.Log("Tile effected by " + currentSpell.spellName + ": X: " + coord.x + " Y: " + coord.y);
                        roomManagerScript.TileClicked(coord.x, coord.y, true);
                    }

                    currentSpell.PlayAnimation(currentX, currentY);
                    tileManagerScript.RemoveAllHighlight();


                    playerScript.DecreaseMana(currentSpell.ManaCost);
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
            if(spell.spellName == playerScript.selectedSpell){
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
