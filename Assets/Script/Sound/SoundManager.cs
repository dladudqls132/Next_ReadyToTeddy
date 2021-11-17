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
    private AudioSource[] allAudioSources;
    private bool isPause;

    // Start is called before the first frame update
    public void Init()
    {
        // allAudioSources = FindObjectsOfType<AudioSource>();
    }

    public void Start()
    {
        allAudioSources = FindObjectsOfType<AudioSource>();
    }

    public void SetPauseAll(bool value)
    {
        if (value)
        {
            foreach (AudioSource audios in allAudioSources)
            {
                audios.Pause();
            }
        }
        else
        {
            foreach (AudioSource audios in allAudioSources)
            {

                audios.UnPause();
            }
        }

        isPause = value;
    }

    public void SetSlowAll(bool value)
    {
        if (value)
        {
            foreach (AudioSource audios in allAudioSources)
            {
                if (audioSource_BGM != audios)
                    audios.pitch = 0.1f;
                else
                    audios.pitch = 0.5f;
                //audios.Pause();
            }
        }
        else
        {
            foreach (AudioSource audios in allAudioSources)
            {

                audios.pitch = 1f;
                //audios.UnPause();
            }
        }
    }

    public void AudioPlayOneShot(SoundType soundName)
    {
        if (audioSourceController_SFX.GetAudioSource() == null || isPause) return;

        if (!audioSourceController_SFX.GetAudioSource().gameObject.activeSelf)
            audioSourceController_SFX.GetAudioSource().gameObject.SetActive(true);

        audioSource_SFX = audioSourceController_SFX.GetAudioSource().GetComponent<AudioSource>();
        audioSource_SFX.Stop();

        audioSource_SFX.loop = false;
        audioSource_SFX.pitch = Random.Range(soundInfo.GetInfo(soundName).pitch_min, soundInfo.GetInfo(soundName).pitch_max);
        audioSource_SFX.volume = Random.Range(soundInfo.GetInfo(soundName).volume - 0.01f, soundInfo.GetInfo(soundName).volume + 0.01f) * GameManager.Instance.GetSettings().data.mainVolume * GameManager.Instance.GetSettings().data.effectVolume;
        audioSource_SFX.clip = soundInfo.GetInfo(soundName).clip;
        audioSource_SFX.Play();

        audioSource_SFX.GetComponent<Sound>().SetInfo(soundInfo.GetInfo(soundName).volume, audioSource_SFX.pitch);
    }

    public AudioSource AudioPlayOneShot(SoundType soundName, bool loop)
    {
        if (audioSourceController_SFX.GetAudioSource() == null || isPause) return null;

        if (!audioSourceController_SFX.GetAudioSource().gameObject.activeSelf)
            audioSourceController_SFX.GetAudioSource().gameObject.SetActive(true);

        audioSource_SFX = audioSourceController_SFX.GetAudioSource().GetComponent<AudioSource>();
        audioSource_SFX.Stop();

        audioSource_SFX.pitch = Random.Range(soundInfo.GetInfo(soundName).pitch_min, soundInfo.GetInfo(soundName).pitch_max);
        audioSource_SFX.volume = Random.Range(soundInfo.GetInfo(soundName).volume - 0.01f, soundInfo.GetInfo(soundName).volume + 0.01f) * GameManager.Instance.GetSettings().data.mainVolume * GameManager.Instance.GetSettings().data.effectVolume;
        //audioSource_SFX.PlayOneShot(soundInfo.GetInfo(soundName).clip);
        audioSource_SFX.clip = soundInfo.GetInfo(soundName).clip;
        audioSource_SFX.Play();
        audioSource_SFX.loop = loop;

        audioSource_SFX.GetComponent<Sound>().SetInfo(soundInfo.GetInfo(soundName).volume, audioSource_SFX.pitch);

        return audioSource_SFX;
    }

    public void AudioPlayOneShot3D(SoundType soundName, Vector3 pos, bool loop)
    {
        if (audioSourceController_SFX_3D.GetAudioSource() == null || isPause) return;

        if (!audioSourceController_SFX_3D.GetAudioSource().gameObject.activeSelf)
            audioSourceController_SFX_3D.GetAudioSource().gameObject.SetActive(true);

        audioSource_SFX = audioSourceController_SFX_3D.GetAudioSource().GetComponent<AudioSource>();
        audioSource_SFX.transform.SetParent(null);
        audioSource_SFX.Stop();

        audioSource_SFX.transform.position = pos;
        audioSource_SFX.pitch = Random.Range(soundInfo.GetInfo(soundName).pitch_min, soundInfo.GetInfo(soundName).pitch_max);
        audioSource_SFX.loop = loop;
        audioSource_SFX.volume = Random.Range(soundInfo.GetInfo(soundName).volume- 0.01f, soundInfo.GetInfo(soundName).volume + 0.01f) * GameManager.Instance.GetSettings().data.mainVolume * GameManager.Instance.GetSettings().data.effectVolume;
        //audioSource_SFX.PlayOneShot(soundInfo.GetInfo(soundName).clip);
        audioSource_SFX.clip = soundInfo.GetInfo(soundName).clip;
        audioSource_SFX.Play();

        audioSource_SFX.GetComponent<Sound>().SetInfo(soundInfo.GetInfo(soundName).volume, audioSource_SFX.pitch);
    }

    public void AudioPlayOneShot3D(SoundType soundName, Transform parent, bool loop)
    {
        if (audioSourceController_SFX_3D.GetAudioSource() == null || isPause) return;

        if (!audioSourceController_SFX_3D.GetAudioSource().gameObject.activeSelf)
            audioSourceController_SFX_3D.GetAudioSource().gameObject.SetActive(true);

        audioSource_SFX = audioSourceController_SFX_3D.GetAudioSource().GetComponent<AudioSource>();
        audioSource_SFX.transform.SetParent(null);
        audioSource_SFX.Stop();

        audioSource_SFX.transform.position = parent.position;
        audioSource_SFX.transform.SetParent(parent);
        audioSource_SFX.pitch = Random.Range(soundInfo.GetInfo(soundName).pitch_min, soundInfo.GetInfo(soundName).pitch_max);
        audioSource_SFX.loop = loop;
        audioSource_SFX.volume = Random.Range(soundInfo.GetInfo(soundName).volume - 0.01f, soundInfo.GetInfo(soundName).volume + 0.01f) * GameManager.Instance.GetSettings().data.mainVolume * GameManager.Instance.GetSettings().data.effectVolume;
        audioSource_SFX.clip = soundInfo.GetInfo(soundName).clip;
        audioSource_SFX.Play();

        audioSource_SFX.GetComponent<Sound>().SetInfo(soundInfo.GetInfo(soundName).volume, audioSource_SFX.pitch);
    }

    public void AudioPlayBGM(BGMSoundType soundName, bool loop)
    {
        if (audioSource_BGM == null || isPause) return;

        audioSource_BGM.Stop();

        audioSource_BGM.clip = bgmSoundInfo.GetInfo(soundName).clip;
        audioSource_BGM.loop = loop;

        audioSource_BGM.volume = bgmSoundInfo.GetInfo(soundName).volume * GameManager.Instance.GetSettings().data.mainVolume * GameManager.Instance.GetSettings().data.backgroundVolume;
        bgm = soundName;

        audioSource_BGM.Play();
    }

    float currentBGMVolume;
    IEnumerator StopBGM()
    {
        currentBGMVolume = audioSource_BGM.volume;
        while(true)
        {
            audioSource_BGM.volume -= currentBGMVolume * Time.deltaTime / 10;

            if(audioSource_BGM.volume <= 0)
            {
                audioSource_BGM.Stop();
                yield break;
            }

            yield return null;
        }
    }

    public void AudioStopBGM()
    {
        StartCoroutine(StopBGM());
    }

    public AudioSource GetBGMSource()
    {
        return audioSource_BGM;
    }

    public void UpdateSetting()
    {
        if (allAudioSources != null)
        {
            if (allAudioSources.Length > 0)
            {
                foreach (AudioSource audios in allAudioSources)
                {
                    if (audios.GetComponent<Sound>() != null)
                        audios.volume = audios.GetComponent<Sound>().volume * GameManager.Instance.GetSettings().data.mainVolume * GameManager.Instance.GetSettings().data.effectVolume;
                }
            }
        }

        //audioSource_SFX.volume = soundInfo.GetInfo(soundName).volume * GameManager.Instance.GetSettings().data.mainVolume * GameManager.Instance.GetSettings().data.effectVolume;
        if (audioSource_BGM != null)
            audioSource_BGM.volume = bgmSoundInfo.GetInfo(bgm).volume * GameManager.Instance.GetSettings().data.mainVolume * GameManager.Instance.GetSettings().data.backgroundVolume;
    }
}
