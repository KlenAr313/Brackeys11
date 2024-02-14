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
    [SerializeField] private Player playerScript;
    [SerializeField] private RoomManager roomManagerScript;
    [SerializeField] private TileManager tileManagerScript;
    [SerializeField] private List<SpellBase> spells;

    void Start(){
        playerScript = GameObject.Find("Player").GetComponent<Player>();
        roomManagerScript = GameObject.Find("Room Manager").GetComponent<RoomManager>();
        tileManagerScript = GameObject.Find("Tile Manager").GetComponent<TileManager>();

        GameObject spellsObj = this.gameObject.transform.Find("Spells").gameObject;

        Component[] components = spellsObj.GetComponents(typeof(Component));
        foreach(Component comp in components){
            if(comp.ToString() != "Spells (UnityEngine.Transform)"){
                spells.Add((SpellBase)comp);
            }
        }

        //Debug miatt true, false legyen alapból!
#if DEBUG
        //isFighting = true;
#else
        isFighting = false;
#endif
    }

    public void TileClicked(int posX, int posY){
        if(isFighting){
            if(turnNumber == 0){
                //todo: nem hardcode-olni a 0-t
                foreach(Tuple<int,int> coord in spells[0].Cast(posX, posY)){
                    roomManagerScript.TileClicked(coord.Item1, coord.Item2, true);
                }
            }
        }
    }

    public void TileHighlighter(int posX, int posY){
        if(isFighting){
            if(turnNumber == 0){
                //todo: nem hardcode-olni a 0-t
                foreach(Tuple<int,int> coord in spells[0].Cast(posX, posY)){
                    tileManagerScript.highlightSpellPreview(coord.Item1, coord.Item2);
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
}
