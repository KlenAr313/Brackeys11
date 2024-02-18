using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : SpellBase
{
    public void Start(){
        base.particlePrefab = Resources.Load<ParticleSystem>("Effects/HealParticles");
        animationTime = particlePrefab.main.duration;
    }

    public override string GetDescription(){
        return "Name: " + this.spellName + "\nCost: " + this.manaCost + "\nHealth received: " + (Player.GetPlayerBaseDamage() * damageModifier) + "\nDesc: " +  description;
    }
}
