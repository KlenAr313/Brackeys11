using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellBase : MonoBehaviour
{
    [SerializeField] public string spellName;
    [SerializeField] protected int damageModifier;
    [SerializeField] protected int cost;

    [SerializeField] protected List<Vector2Int> extraAffectedTiles;

    public int DamageModifier { get => damageModifier; set => damageModifier = value; }
    public int Cost { get => cost; set => cost = value; }


    public virtual List<Vector2Int> Cast(int posX, int posY){
        List<Vector2Int> coords = new List<Vector2Int>();

        //Debug.Log("You cast a fireball spell at: X: " + posX + " Y: " + posY);

        //Affected Tiles
        coords.Add(new Vector2Int(posX, posY));
        foreach(Vector2Int coord in extraAffectedTiles){
            coords.Add(new Vector2Int(posX + coord.x, posY + coord.y));
        }

        return coords;
    }

}
