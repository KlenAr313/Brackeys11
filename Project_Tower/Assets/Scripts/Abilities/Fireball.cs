using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Fireball : SpellBase
{
    
    public override List<Tuple<int,int>> Cast(int posX, int posY){

        List<Tuple<int,int>> coords = new List<Tuple<int, int>>();

        //Debug.Log("You cast a fireball spell at: X: " + posX + " Y: " + posY);

        //Affected Tiles
            coords.Add(new Tuple<int, int>(posX, posY));
            coords.Add(new Tuple<int, int>(posX + 1, posY));
            coords.Add(new Tuple<int, int>(posX + 2, posY));

        return coords;
    }
}
