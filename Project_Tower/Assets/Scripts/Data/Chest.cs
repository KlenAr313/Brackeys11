using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : AbstractInteractable
{
    public string clue;

    public Chest(bool isEnable, string clue = "")
    {
        this.isEnable = isEnable;
        if(isEnable){
            this.clue = clue;
        }
    }
}
