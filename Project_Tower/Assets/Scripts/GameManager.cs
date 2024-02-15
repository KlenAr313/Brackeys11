using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //0 mindig a player
    [SerializeField] public bool isPlayerTurn;
    [SerializeField] public bool isFighting;
    [SerializeField] public GameObject playerObj;
    [SerializeField] public Player playerScript;
    [SerializeField] public RoomManager roomManagerScript;
    [SerializeField] public TileManager tileManagerScript;
    [SerializeField] private List<SpellBase> spellList;
    [SerializeField] public SpellBase currentSpell = null;
    [SerializeField] private CombatManager combatManagerScript;

    private int currentX;
    private int currentY;

    public event Action SpellRefreshed;

    void Start(){
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
            }
        }

        RefreshCurrentSpell();

        //Debug miatt true, false legyen alapból!
#if DEBUG
        //isFighting = true;
#else
        isFighting = false;
#endif
    }

    public void TileClicked(int posX, int posY){
        currentX = posX;
        currentY = posY;
        if(isFighting){
            //Player köre
            if(isPlayerTurn){
                if(currentSpell != null){
                    foreach(Vector2Int coord in currentSpell.Cast(posX, posY)){
                        roomManagerScript.TileClicked(coord.x, coord.y, true);
                    }
                    combatManagerScript.PlayerTakeTurn();
                }
            }
        }
        else
        {
            roomManagerScript.TileClicked(posX, posY, false);
        }
    }

    public void TileHighlighter(int posX, int posY){
        currentX = posX;
        currentY = posY;
        if(isFighting){
            //Player köre
            if(isPlayerTurn){
                if(currentSpell != null){
                    foreach(Vector2Int coord in currentSpell.Cast(posX, posY)){
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

    public void EnemyStrikes(List<Vector2> positions, int amount)
    {
        foreach(Vector2 pos in positions){
            if(pos.x == playerScript.PosX && pos.y == playerScript.PosY){
                playerScript.GetDamaged(amount);
            }
        }
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

    public int GetSpellDamage(string spellName){
        foreach(SpellBase spell in spellList){
            if(spell.spellName == spellName){
                return spell.DamageModifier;
            }
        }
        return 0;
    }

#if DEBUG
    void Update(){
        if(Input.GetKeyDown(KeyCode.K)){
            Debug.Log("Combat started!");
            isFighting = true;
            //isPlayerTurn = true;
            combatManagerScript.StartCombat();
        }

        if(Input.GetKeyDown(KeyCode.L)){
            Debug.Log("Combat ended!");
            isFighting = false;
            tileManagerScript.RemoveAllHighlight();
            combatManagerScript.EndCombat();
        }
    } 
#endif

}
