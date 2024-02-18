using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FlameVortex : SpellBase
{
    public void Start(){
        base.particlePrefab = Resources.Load<ParticleSystem>("Effects/FlameVortexParticles");
        animationTime = particlePrefab.main.duration;
    }

    public override string GetDescription(){
        return "Name: " + "Flame Vortex" + "\nCost: " + this.manaCost + "\nDamage: " + (Player.GetPlayerBaseDamage() * damageModifier) + "\nDesc: " +  description;
    }
}
