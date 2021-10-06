using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class VideoController : MonoBehaviour
{
    [SerializeField] private VideoPlayer video;

    [SerializeField] private Video videoInfo;

    [SerializeField] private Text text;


    public void Init()
    {
        this.gameObject.SetActive(false);
    }

    public void PlayVideo(GunType gun)
    {
        switch(gun)
        {
            case GunType.ShotGun:
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0;
                this.gameObject.SetActive(true);
                video.clip = videoInfo.GetVideo(GunType.ShotGun).clip;
                text.text = videoInfo.GetVideo(GunType.ShotGun).text;
                video.Play();
                break;
            case GunType.ChainLightning:
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0;
                this.gameObject.SetActive(true);
                video.clip = videoInfo.GetVideo(GunType.ChainLightning).clip;
                text.text = videoInfo.GetVideo(GunType.ChainLightning).text;
                video.Play();
                break;
            case GunType.Flamethrower:
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0;
                this.gameObject.SetActive(true);
                video.clip = videoInfo.GetVideo(GunType.Flamethrower).clip;
                text.text = videoInfo.GetVideo(GunType.Flamethrower).text;
                video.Play();
                break;
        }
    }

    public void StopVideo()
    {
        Time.timeScale = 1;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        video.Stop();
        video.clip = null;

        this.gameObject.SetActive(false);
    }
}
