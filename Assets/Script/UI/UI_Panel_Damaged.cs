using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Panel_Damaged : MonoBehaviour
{
    [SerializeField] private Image image;

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.GetPlayer().GetIsDamaged())
        {
            image.color = Color.Lerp(image.color, Color.white, Time.deltaTime * 12);
        }
        else
        {
            image.color = Color.Lerp(image.color, new Color(1, 1, 1, 0), Time.deltaTime * 12);
        }
    }
}
