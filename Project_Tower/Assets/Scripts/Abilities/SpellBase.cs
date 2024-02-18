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
    [SerializeField] public Sprite icon;
    [SerializeField] public SFXPlay sfx;

    public float animationTime;
    [SerializeField] protected string description;

    [SerializeField] protected ParticleSystem particlePrefab;
    [SerializeField] protected List<Vector2Int> extraAffectedTiles;

    public int DamageModifier { get => damageModifier; set => damageModifier = value; }
    public int ManaCost { get => manaCost; set => manaCost = value; }


    public virtual List<Vector2Int> Cast(int posX, int posY){
        List<Vector2Int> coords = new List<Vector2Int>();

        //Affected Tiles
        coords.Add(new Vector2Int(posX, posY));
        foreach(Vector2Int coord in extraAffectedTiles){
            coords.Add(new Vector2Int(posX + coord.x, posY + coord.y));
        }

        return coords;
    }

    public virtual float PlayAnimation(int posX, int posY){
        ParticleSystem particle = Instantiate(particlePrefab, new Vector3(posX, posY, -1), Quaternion.identity);
        
        particle.Play();
        
        return animationTime;
    }

    public virtual void PlaySound()
    {
        sfx.PlayFX(spellName);
    }

    public virtual string GetDescription(){
        return "Name: " + this.spellName + "\nCost: " + this.manaCost + "\nDamage: " + (Player.GetPlayerBaseDamage() * damageModifier) + "\nDesc: " +  description;
    }

}
