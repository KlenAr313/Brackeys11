using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicScript : MonoBehaviour
{

    AudioSource title;
    AudioSource boring;
    AudioSource combat;
    AudioSource source;
    AudioSource[] aSources;
    int i = 2;

    void Start()
    {
        aSources = GetComponents<AudioSource>();
        combat = aSources[2];
        combat.volume = 0.4f;
        StartCoroutine(Fade(true, combat, 2f, 1f));
        StartCoroutine(Fade(false, combat, 2f, 0f));
    }

    void Update()
    {
        if (i < 0 || i > 2)
        {
            i = 2;
        }
        source = aSources[i];
        if (!source.isPlaying)
        {
            source.Play();
            StartCoroutine(Fade(true, source, 2f, 1f));
            StartCoroutine(Fade(false, source, 2f, 0f));
        }
    }

    public IEnumerator Fade(bool fadeIn, AudioSource source, float duration, float targetVolume)
    {
        if (!fadeIn)
        {
            double lengthOfSource = (double)source.clip.samples / source.clip.frequency;
            yield return new WaitForSecondsRealtime((float)(lengthOfSource - duration));
        }

        float time = 0f;
        float startVol = source.volume;
        while (time < duration)
        {
            time += Time.deltaTime;
            source.volume = Mathf.Lerp(startVol, targetVolume, time / duration);
            yield return null;
        }

        

        yield break;
    }

    void executeWait(float aux)
    {
        StartCoroutine(Wait(aux));
    }

    IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
}
