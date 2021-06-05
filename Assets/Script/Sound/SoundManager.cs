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
    [SerializeField] private BGMSoundInfo bgmSoundInfo;

    [SerializeField] private AudioSourceController audioSourceController_SFX;
    [SerializeField] private AudioSourceController audioSourceController_SFX_3D;

    private AudioSource audioSource_SFX;
    //private AudioSource audioSource_SFX_3D;
    [SerializeField] private AudioSource audioSource_BGM;
    private BGMSoundType bgm;

    // Start is called before the first frame update
    public void Init()
    {

    }

    public void SetPauseAll(bool value)
    {

    }

    public void AudioPlayOneShot(SoundType soundName)
    {
        if (audioSourceController_SFX.GetAudioSource() == null) return;

        audioSource_SFX = audioSourceController_SFX.GetAudioSource().GetComponent<AudioSource>();

        audioSource_SFX.pitch = Random.Range(soundInfo.GetInfo(soundName).pitch_min, soundInfo.GetInfo(soundName).pitch_max);
        audioSource_SFX.Stop();
        audioSource_SFX.PlayOneShot(soundInfo.GetInfo(soundName).clip, soundInfo.GetInfo(soundName).volume * GameManager.Instance.GetSettings().data.effectVolume);

    }

    public void AudioPlayOneShot3D(SoundType soundName, Vector3 pos, bool loop)
    {
        if (audioSourceController_SFX_3D.GetAudioSource() == null) return;

        audioSource_SFX = audioSourceController_SFX_3D.GetAudioSource().GetComponent<AudioSource>();

        audioSource_SFX.transform.position = pos;

        audioSource_SFX.pitch = Random.Range(soundInfo.GetInfo(soundName).pitch_min, soundInfo.GetInfo(soundName).pitch_max);

        audioSource_SFX.loop = loop;

        audioSource_SFX.Stop();
        audioSource_SFX.PlayOneShot(soundInfo.GetInfo(soundName).clip, soundInfo.GetInfo(soundName).volume * GameManager.Instance.GetSettings().data.effectVolume);
    }

    public AudioSource AudioPlayOneShot3D_Get(SoundType soundName, Vector3 pos, bool loop)
    {
        if (audioSourceController_SFX_3D.GetAudioSource() == null) return null;

        audioSource_SFX = audioSourceController_SFX_3D.GetAudioSource().GetComponent<AudioSource>();

        audioSource_SFX.transform.position = pos;

        audioSource_SFX.pitch = Random.Range(soundInfo.GetInfo(soundName).pitch_min, soundInfo.GetInfo(soundName).pitch_max);

        audioSource_SFX.loop = loop;

        audioSource_SFX.Stop();
        audioSource_SFX.PlayOneShot(soundInfo.GetInfo(soundName).clip, soundInfo.GetInfo(soundName).volume * GameManager.Instance.GetSettings().data.effectVolume);

        return audioSource_SFX;
    }

    public void AudioPlayOneShot3D(SoundType soundName, Transform parent, bool loop)
    {
        if (audioSourceController_SFX_3D.GetAudioSource() == null) return;

        audioSource_SFX = audioSourceController_SFX_3D.GetAudioSource().GetComponent<AudioSource>();

        audioSource_SFX.transform.position = parent.position;
        audioSource_SFX.transform.SetParent(parent);

        audioSource_SFX.pitch = Random.Range(soundInfo.GetInfo(soundName).pitch_min, soundInfo.GetInfo(soundName).pitch_max);

        audioSource_SFX.loop = loop;

        audioSource_SFX.Stop();
        audioSource_SFX.PlayOneShot(soundInfo.GetInfo(soundName).clip, soundInfo.GetInfo(soundName).volume * GameManager.Instance.GetSettings().data.effectVolume);
    }

    public AudioSource AudioPlayOneShot3D_Get(SoundType soundName, Transform parent, bool loop)
    {
        if (audioSourceController_SFX_3D.GetAudioSource() == null) return null;

        audioSource_SFX = audioSourceController_SFX_3D.GetAudioSource().GetComponent<AudioSource>();

        audioSource_SFX.transform.position = parent.position;
        audioSource_SFX.transform.SetParent(parent);

        audioSource_SFX.pitch = Random.Range(soundInfo.GetInfo(soundName).pitch_min, soundInfo.GetInfo(soundName).pitch_max);

        audioSource_SFX.loop = loop;

        audioSource_SFX.Stop();
        audioSource_SFX.PlayOneShot(soundInfo.GetInfo(soundName).clip, soundInfo.GetInfo(soundName).volume * GameManager.Instance.GetSettings().data.effectVolume);

        return audioSource_SFX;
    }

    public void AudioPlayBGM(BGMSoundType soundName, bool loop)
    {
        audioSource_BGM.clip = bgmSoundInfo.GetInfo(soundName).clip;
        audioSource_BGM.loop = loop;

        audioSource_BGM.volume = bgmSoundInfo.GetInfo(soundName).volume * GameManager.Instance.GetSettings().data.backgroundVolume;
        bgm = soundName;

        audioSource_BGM.Stop();
        audioSource_BGM.Play();
    }

    private void Update()
    {
        audioSource_BGM.volume = bgmSoundInfo.GetInfo(bgm).volume * GameManager.Instance.GetSettings().data.backgroundVolume;
    }
}
