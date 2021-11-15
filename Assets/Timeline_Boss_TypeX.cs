using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class Timeline_Boss_TypeX : MonoBehaviour
{
    [SerializeField] private TimelineAsset[] clip;
    private PlayableDirector pd;
    [SerializeField] private bool isPlay;

    public bool GetIsPlay() { return isPlay; }

    private void Start()
    {
        pd = this.GetComponent<PlayableDirector>();
    }

    public void PlayTimeline(int phase, bool fadeOut)
    {
        pd.playableAsset = clip[phase];
        isPlay = true;

        if (fadeOut)
        {
            SetIsPlayTrue();
            GameManager.Instance.GetFade().SetFade(FadeState.FadeOut, 6);
            StartCoroutine(FadeDelay(0.5f));
        }
        else
        {
            SetIsPlayTrue();
            pd.Play();            
        }
    }

    IEnumerator FadeDelay(float time)
    {
        yield return new WaitForSecondsRealtime(time);

        pd.Play();
    }

    void SetIsPlayTrue()
    {
        Time.timeScale = 0;
        isPlay = true;
    }

    void SetIsPlayFalse()
    {
        Time.timeScale = 1;
        isPlay = false;
    }
}
