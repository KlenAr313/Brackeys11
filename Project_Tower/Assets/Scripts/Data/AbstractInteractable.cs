using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBase: MonoBehaviour
{
    public bool IsEnable;
    public Vector3Int Pos;

    public InteractableBase(bool IsEnable, Vector2Int Pos){
        this.IsEnable = IsEnable;
        this.Pos = new Vector3Int(Pos.x, Pos.y, 0);
    }

    public InteractableBase(InteractableBase taht){
        this.Pos = taht.Pos;
        this.IsEnable = taht.IsEnable;
    }

    public void SetValue(bool IsEnable, Vector2Int Pos){
        this.IsEnable = IsEnable;
        this.Pos = new Vector3Int(Pos.x, Pos.y, 0);
    }
}
