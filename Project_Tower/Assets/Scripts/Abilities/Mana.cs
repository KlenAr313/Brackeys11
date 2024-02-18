using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mana : SpellBase
{
    public void Start(){
        base.particlePrefab = Resources.Load<ParticleSystem>("Effects/ManaParticles");
        animationTime = particlePrefab.main.duration;
        this.description = "A simple way to\nrecover some mana,\nbut your turn is the price";
    }

    public override string GetDescription(){
        return "Name: " + "Recover Mana" + "\nCost: " + this.manaCost + "\nMana received: " + (Player.GetPlayerBaseDamage() * damageModifier) + "\nDesc: " +  description;
    }
}
