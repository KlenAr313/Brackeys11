using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : SpellBase
{
    public void Start(){
        base.particlePrefab = Resources.Load<ParticleSystem>("Effects/MeteorParticles");
        animationTime = particlePrefab.main.duration;
    }
}
