using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceController : MonoBehaviour
{
    enum AudioSourceType
    {
        Normal,
        World
    }

    private List<GameObject> audioSources = new List<GameObject>();

    [SerializeField] private GameObject audioSource;
    [SerializeField] private int audioSourceNum;
    // Start is called before the first frame update
    void Awake()
    {

        for (int i = 0; i < audioSourceNum; i++)
        {
            GameObject temp = Instantiate(audioSource, this.transform);
            audioSources.Add(temp);
        }

    }

    public GameObject GetAudioSource()
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            if (!audioSources[i].GetComponent<AudioSource>().isPlaying || !audioSources[i].activeSelf)
            {

                //audioSources[i].GetComponent<AudioSource>().Stop();
                return audioSources[i].gameObject;
            }

        }

        return null;
    }
}
