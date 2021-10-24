using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Dash : MonoBehaviour
{
    [SerializeField] private GameObject[] images;

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < images.Length; i++)
        {
            images[i].SetActive(false);
        }

        for(int i = 0; i < GameManager.Instance.GetPlayer().GetCurrentDashCount(); i++)
        {
            images[i].SetActive(true);
        }

    }
}
