using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Dash : MonoBehaviour
{
    [SerializeField] private Text text;

    // Update is called once per frame
    void Update()
    {
        text.text = GameManager.Instance.GetPlayer().GetCurrentDashCount().ToString();
    }
}
