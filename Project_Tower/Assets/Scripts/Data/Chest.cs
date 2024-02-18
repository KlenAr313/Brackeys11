using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : InteractableBase, IInteractable
{
    public string Clue = String.Empty;

    public Chest(bool IsEnable, Vector2Int Pos, string Clue = "") : base(IsEnable, Pos)
    {
        this.Clue = Clue;
    }

    public Chest(Chest taht) : base(taht)
    {
        this.Clue = taht.Clue;
    }

    public void CreateChest(bool IsEnable, Vector2Int Pos, string Clue = "")
    {
        base.SetValue(IsEnable, Pos);
        if(IsEnable){
            this.Clue = Clue;
        }
    }

    public void CopyData(Chest that){
        this.IsEnable = that.IsEnable;
        this.Pos = that.Pos;
        this.Clue = that.Clue;
    }

    public void Click(){
        GameObject.Find("Game Manager").GetComponent<PopUpSystem>().PopUp(Clue);
    }

    public void NewPos(Vector2Int Pos){
        this.Pos = new Vector3Int(Pos.x, Pos.y, -1);
    }
}
