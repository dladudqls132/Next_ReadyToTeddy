using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMController : MonoBehaviour
{
    [SerializeField] private BGMSoundType[] bgmType;
    [SerializeField] private bool playOnWake;
    [SerializeField] private bool isRelay;
    int playNum;

    public void SetIsRelay(bool value)
    {
        isRelay = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        playNum = 0;

        if(playOnWake)
            GameManager.Instance.GetSoundManager().AudioPlayBGM(bgmType[0], false);
    }

    public void StopBGM()
    {
        isRelay = false;
        GameManager.Instance.GetSoundManager().AudioStopBGM();
    }

    public void PlayBGM(int num, bool isRelay)
    {
        if (num >= bgmType.Length) return;

        this.isRelay = isRelay;
        GameManager.Instance.GetSoundManager().AudioPlayBGM(bgmType[num], false);
        playNum = num + 1;
        playNum %= bgmType.Length;
    }

    private void Update()
    {
        if (isRelay)
        {
            if (!GameManager.Instance.GetIsPause())
            {
                if (!GameManager.Instance.GetSoundManager().GetBGMSource().isPlaying)
                {
                    playNum++;
                    playNum %= bgmType.Length;
                    GameManager.Instance.GetSoundManager().AudioPlayBGM(bgmType[playNum], false);
                }
            }
        }
    }
}
