using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMController : MonoBehaviour
{
    [SerializeField] private BGMSoundType bgmType;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.GetSoundManager().AudioPlayBGM(bgmType, true);
    }
}
