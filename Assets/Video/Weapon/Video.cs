using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

[System.Serializable]
public struct VideoInfo
{
    public GunType gunType;
    public VideoClip clip;
    public RenderTexture renderTexture;
    public string text_name;
    public string text_ex;
}

[CreateAssetMenu]
public class Video : ScriptableObject
{
    public VideoInfo[] videoInfos;

    public VideoInfo GetVideo(GunType gunType)
    {
        for(int i = 0; i < videoInfos.Length; i++)
        {
            if(videoInfos[i].gunType.Equals(gunType))
            {
                return videoInfos[i];
            }
        }

        return new VideoInfo();
    }
}
