using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class RoomData
{
    public bool IsWin;
    public int Type;
    public List<AbstractInteractable> Interactables;

    public RoomData(int Type)
    {
        IsWin = false;
        this.Type = Type;
    }
}
