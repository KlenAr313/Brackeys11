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
}
