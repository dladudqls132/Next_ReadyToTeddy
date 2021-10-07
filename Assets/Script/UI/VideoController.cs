using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class VideoController : MonoBehaviour
{
    [SerializeField] private VideoPlayer video;

    [SerializeField] private Video videoInfo;

    [SerializeField] private GameObject name_SG;
    [SerializeField] private GameObject name_CL;
    [SerializeField] private GameObject name_FT;

    [SerializeField] private Text text_ex;


    public void Init()
    {
        this.gameObject.SetActive(false);
    }

    void ResetName()
    {
        name_SG.SetActive(false);
        name_CL.SetActive(false);
        name_FT.SetActive(false);
    }

    public void PlayVideo(GunType gun)
    {
        ResetName();

        switch (gun)
        {
            case GunType.ShotGun:
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0;
                GameManager.Instance.GetSoundManager().SetPauseAll(true);
                this.gameObject.SetActive(true);
                video.clip = videoInfo.GetVideo(GunType.ShotGun).clip;
                name_SG.SetActive(true);
                text_ex.text = videoInfo.GetVideo(GunType.ShotGun).text_ex;
                video.Play();
                break;
            case GunType.ChainLightning:
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0;
                GameManager.Instance.GetSoundManager().SetPauseAll(true);
                this.gameObject.SetActive(true);
                video.clip = videoInfo.GetVideo(GunType.ChainLightning).clip;
                name_CL.SetActive(true);
                text_ex.text = videoInfo.GetVideo(GunType.ChainLightning).text_ex;
                video.Play();
                break;
            case GunType.Flamethrower:
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0;
                GameManager.Instance.GetSoundManager().SetPauseAll(true);
                this.gameObject.SetActive(true);
                video.clip = videoInfo.GetVideo(GunType.Flamethrower).clip;
                name_FT.SetActive(true);
                text_ex.text = videoInfo.GetVideo(GunType.Flamethrower).text_ex;
                video.Play();
                break;
        }
    }

    public void StopVideo()
    {
        Time.timeScale = 1;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        GameManager.Instance.GetSoundManager().SetPauseAll(false);

        video.Stop();
        video.clip = null;

        this.gameObject.SetActive(false);
    }
}
