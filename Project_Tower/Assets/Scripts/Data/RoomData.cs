using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class RoomData
{
    public bool IsWon;
    public int Type;
    public List<InteractableBase> Interactables;

    public bool[] Doors;

    public RoomData(){
        Type = 0;
    }
    public RoomData(int Type)
    {
        IsWon = false;
        this.Type = Type;
        Interactables = new List<InteractableBase>();
        randomiseChest();
    }

    private bool randomiseChest(){
        Chest chest = new Chest(true, new Vector2Int(1,1), "A kankalin sötétben virágzik");  
        Interactables.Add(chest);  
        return true;
    }
}
