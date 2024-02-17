using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public abstract class SpellBase : MonoBehaviour
{
    [SerializeField] public string spellName;
    [SerializeField] protected int damageModifier;
    [SerializeField] protected int manaCost;

    public float animationTime;

    [SerializeField] protected ParticleSystem particlePrefab;
    [SerializeField] protected List<Vector2Int> extraAffectedTiles;

    public int DamageModifier { get => damageModifier; set => damageModifier = value; }
    public int ManaCost { get => manaCost; set => manaCost = value; }


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

    public virtual float PlayAnimation(int posX, int posY){
        ParticleSystem particle = Instantiate(particlePrefab, new Vector3(posX, posY, 0), Quaternion.identity);
        
        particle.Play();
        
        return animationTime;
    }

}
