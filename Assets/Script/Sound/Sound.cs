using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    public float volume;
    public float pitch;

    public void SetInfo(float volume, float pitch)
    {
        this.volume = volume;
        this.pitch = pitch;
    }
}
