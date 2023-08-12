using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioComponent : MonoBehaviour
{
    public List<AudioClip> audioClips;

    void Start()
    {

    }

    public void PlayRandomSound(Transform _parent, Vector3 startPos, float volumeModifier)
    {
        GameObject g = new GameObject("AudioClip");
        g.transform.parent = _parent;
        g.transform.position = startPos;

        AudioSource source = g.AddComponent<AudioSource>();
        source.volume = Controls.instance.volume;

        //random sound from public list
        source.clip = audioClips[Random.Range(0,audioClips.Count)];
        source.volume = Controls.instance.volume * volumeModifier;

        source.Play();

        DeleteAudio(g, source.clip.length);
    }

    public void PlaySound(int soundIndex, Transform _parent, Vector3 startPos, float volumeModifier)
    {
        GameObject g = new GameObject("AudioClip");
        g.transform.parent = _parent;
        g.transform.position = startPos;

        AudioSource source = g.AddComponent<AudioSource>();
        source.volume = Controls.instance.volume * volumeModifier;

        //random sound from public list
        source.clip = audioClips[soundIndex];

        source.Play();

        DeleteAudio(g, source.clip.length);
    }

    IEnumerator DeleteAudio(GameObject audioObj, float delay)
    {
        yield return new WaitForSeconds(delay);

        Destroy(audioObj);
    }
    
   
}
