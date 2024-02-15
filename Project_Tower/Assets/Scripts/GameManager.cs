using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //0 mindig a player
    [SerializeField] private int turnNumber;
    [SerializeField] private bool isFighting;
    [SerializeField] public Player playerScript;
    [SerializeField] private RoomManager roomManagerScript;
    [SerializeField] private TileManager tileManagerScript;
    [SerializeField] private List<SpellBase> spellList;
    [SerializeField] public SpellBase currentSpell = null;

    private int currentX;
    private int currentY;

    public event Action SpellRefreshed;

    void Start(){
        playerScript = GameObject.Find("Player").GetComponent<Player>();
        roomManagerScript = GameObject.Find("Room Manager").GetComponent<RoomManager>();
        tileManagerScript = GameObject.Find("Tile Manager").GetComponent<TileManager>();

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
            if(turnNumber == 0){
                if(currentSpell != null){
                    foreach(Vector2Int coord in currentSpell.Cast(posX, posY)){
                        roomManagerScript.TileClicked(coord.x, coord.y, true);
                    }
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
            if(turnNumber == 0){
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
    }

    public void EndFight(){
        this.isFighting = false;
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

}
