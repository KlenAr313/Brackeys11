using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlay : MonoBehaviour
{

    AudioSource meteor;
    AudioSource vortex;
    AudioSource fireball;

    void Start(){
        AudioSource[] aSources = GetComponents<AudioSource>();
        meteor = aSources[0];
        vortex = aSources[1];
        fireball = aSources[2];
    }

    public void PlayFX(string name)
    {
        if (name == "Meteor")
        {
            meteor.Play();
        }
        else if (name == "FlameVortex")
        {
            vortex.Play();
        }
        else
        {
            fireball.Play();
        }
    }

}
