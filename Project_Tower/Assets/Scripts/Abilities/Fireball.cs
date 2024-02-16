using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Fireball : SpellBase
{
    public void Start(){
        base.particlePrefab = Resources.Load<ParticleSystem>("Effects/FireballParticles");
        animationTime = particlePrefab.main.duration;
    }

    
    public override List<Vector2Int> Cast(int posX, int posY){

        return base.Cast(posX, posY);
    }
}
