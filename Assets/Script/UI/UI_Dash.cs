using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Dash : MonoBehaviour
{
    [SerializeField] private Text text;
    [SerializeField] private Text text_background;

    // Update is called once per frame
    void Update()
    {
        text.text = GameManager.Instance.GetPlayer().GetCurrentDashCount().ToString();
        text_background.text = GameManager.Instance.GetPlayer().GetCurrentDashCount().ToString();
    }
}
