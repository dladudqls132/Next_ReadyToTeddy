using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMController : MonoBehaviour
{
    [SerializeField] private BGMSoundType[] bgmType;
    int playNum;

    // Start is called before the first frame update
    void Start()
    {
        playNum = 0;
        GameManager.Instance.GetSoundManager().AudioPlayBGM(bgmType[0], false);
    }

    private void Update()
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
