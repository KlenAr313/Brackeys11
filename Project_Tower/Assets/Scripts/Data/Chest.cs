using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : AbstractInteractable
{
    public string Clue;

    public Chest(bool IsEnable, Vector2Int Pos, string Clue = "") : base(IsEnable, Pos)
    {
        if(IsEnable){
            this.Clue = Clue;
        }
    }
}
