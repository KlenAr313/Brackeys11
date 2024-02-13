using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //0 mindig a player
    [SerializeField] private int turnNumber;
    [SerializeField] private bool isFighting;
    [SerializeField] private Player playerScript;
    [SerializeField] private RoomManager roomManagerScript;
    [SerializeField] private List<SpellBase> spells;
    void Start(){
        playerScript = GameObject.Find("Player").GetComponent<Player>();
        roomManagerScript = GameObject.Find("Room Manager").GetComponent<RoomManager>();

        GameObject spellsObj = this.gameObject.transform.Find("Spells").gameObject;

        Component[] components = spellsObj.GetComponents(typeof(Component));
        foreach(Component comp in components){
            if(comp.ToString() != "Spells (UnityEngine.Transform)"){
                spells.Add((SpellBase)comp);
            }
        }

        //Debug miatt true, false legyen alapból!
        isFighting = true;
    }

    public void TileClicked(int posX, int PosY){
        if(isFighting){
            if(turnNumber == 0){
                foreach(Tuple<int,int> coord in spells[0].Cast(posX, PosY)){
                    roomManagerScript.TileClicked(coord.Item1, coord.Item2, true);
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
