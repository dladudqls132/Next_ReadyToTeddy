using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceController : MonoBehaviour
{
    private List<AudioSource> audioSources = new List<AudioSource>();
    [SerializeField] private int audioSourceNum;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < audioSourceNum; i++)
        {
            AudioSource temp = this.GetComponent<AudioSource>();
            AudioSource temp2 = this.gameObject.AddComponent<AudioSource>();
            temp2.playOnAwake = false;
            temp2.spatialBlend = temp.spatialBlend;
            audioSources.Add(temp2);
        }
    }

    public AudioSource GetAudioSource()
    {
        for(int i = 0; i < audioSources.Count; i++)
        {
            if (!audioSources[i].isPlaying)
                return audioSources[i];
        }

        return null;
    }
}
