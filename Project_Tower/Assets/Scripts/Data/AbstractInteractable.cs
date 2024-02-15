using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractInteractable
{
    public bool IsEnable;
    public Vector3Int Pos;

    public AbstractInteractable(bool IsEnable, Vector2Int Pos){
        this.IsEnable = IsEnable;
        this.Pos = new Vector3Int(Pos.x, Pos.y, 0);
    }
}
