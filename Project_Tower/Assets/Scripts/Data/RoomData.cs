using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class RoomData
{
    public static List<string> cluePool;
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
        Chest chest;
        if(Type == 1){
            chest = new Chest(true, new Vector2Int(13,1), "Click any door to go to the next room"); 
        }
        else{
            int r = Random.Range(0, cluePool.Count);
            chest = new Chest(true, new Vector2Int(1,1), cluePool[r]);
            cluePool.RemoveAt(r); 
        }
        Interactables.Add(chest);
        return true;
    }
}
