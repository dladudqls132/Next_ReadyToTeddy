using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioSourceType
{
    SFX,
    BGM
}

public class SoundManager : MonoBehaviour
{
    [SerializeField] private SoundInfo soundInfo;
    [SerializeField] private AudioSource audioSource_SFX;
    [SerializeField] private AudioSource audioSource_SFX_3D;
    [SerializeField] private AudioSource audioSource_BGM;

    // Start is called before the first frame update
    public void Init()
    {
        if (Camera.main.GetComponent<FPPCamController>() != null)
        {
            audioSource_SFX = Camera.main.GetComponent<FPPCamController>().GetAudioSource_SFX();
            audioSource_SFX_3D = Camera.main.GetComponent<FPPCamController>().GetAudioSource_SFX_3D();
            audioSource_BGM = Camera.main.GetComponent<FPPCamController>().GetAudioSource_BGM();
        }
    }

    public void AudioPlayOneShot(AudioSourceType audioSourceType, SoundType soundName)
    {
        if(audioSourceType == AudioSourceType.SFX)
        {
            audioSource_SFX.pitch = Random.Range(soundInfo.GetInfo(soundName).pitch_min, soundInfo.GetInfo(soundName).pitch_max);
            audioSource_SFX.PlayOneShot(soundInfo.GetInfo(soundName).clip, soundInfo.GetInfo(soundName).volume * GameManager.Instance.GetSettings().data.effectVolume);
        }
        else if(audioSourceType == AudioSourceType.BGM)
        {
            audioSource_BGM.pitch = Random.Range(soundInfo.GetInfo(soundName).pitch_min, soundInfo.GetInfo(soundName).pitch_max);
            audioSource_BGM.PlayOneShot(soundInfo.GetInfo(soundName).clip, soundInfo.GetInfo(soundName).volume * GameManager.Instance.GetSettings().data.effectVolume);
        }
    }

    public void AudioPlayOneShot3D(AudioSourceType audioSourceType, SoundType soundName)
    {
        if (audioSourceType == AudioSourceType.SFX)
        {
            audioSource_SFX_3D.pitch = Random.Range(soundInfo.GetInfo(soundName).pitch_min, soundInfo.GetInfo(soundName).pitch_max);
            audioSource_SFX_3D.PlayOneShot(soundInfo.GetInfo(soundName).clip, soundInfo.GetInfo(soundName).volume * GameManager.Instance.GetSettings().data.effectVolume);
        }
        else if (audioSourceType == AudioSourceType.BGM)
        {
            audioSource_BGM.pitch = Random.Range(soundInfo.GetInfo(soundName).pitch_min, soundInfo.GetInfo(soundName).pitch_max);
            audioSource_BGM.PlayOneShot(soundInfo.GetInfo(soundName).clip, soundInfo.GetInfo(soundName).volume * GameManager.Instance.GetSettings().data.effectVolume);
        }
    }
}
