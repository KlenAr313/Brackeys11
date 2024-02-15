using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractInteractable
{
    public bool IsEnable;
    public Vector2Int Pos;

    public AbstractInteractable(bool IsEnable, Vector2Int Pos){
        this.IsEnable = IsEnable;
        this.Pos = Pos;
    }
}
