using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellBase : MonoBehaviour
{
    [SerializeField] protected int damageModifier;
    [SerializeField] protected int cost;

    public int DamageModifier { get => damageModifier; set => damageModifier = value; }
    public int Cost { get => cost; set => cost = value; }


    public virtual List<Tuple<int,int>> Cast(int posX, int posY){
        return null;
    }

}
