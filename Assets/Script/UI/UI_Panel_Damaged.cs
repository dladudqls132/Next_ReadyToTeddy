using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Panel_Damaged : MonoBehaviour
{
    [SerializeField] private Image image;
    private bool isHeartBeat;
    private AudioSource audioSource;

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.GetPlayer().GetIsDamaged())
        {
            image.color = Color.Lerp(image.color, Color.red, Time.deltaTime * 12);
        }
        else
        {
            if (GameManager.Instance.GetPlayer().GetCurrentHp() <= 30)
            {
                image.color = Color.Lerp(image.color, new Color(1, 0, 0, 0.6f), Time.deltaTime * 12);

                if (!isHeartBeat)
                {
                    audioSource = GameManager.Instance.GetSoundManager().AudioPlayOneShot(SoundType.HeartBeat, true);
                    isHeartBeat = true;
                }
            }
            else
            {
                if (audioSource != null)
                {
                    audioSource.clip = null;
                    audioSource.Stop();
                    audioSource = null;
                }

                isHeartBeat = false;
                image.color = Color.Lerp(image.color, new Color(1, 0, 0, 0), Time.deltaTime * 12);
            }
        }
    }
}
