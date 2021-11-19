using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_RemainingEnemy : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    public void Init()
    {
        this.gameObject.SetActive(false);
    }

    public void SetNum(int num)
    {
        text.text = "Remaining Enemy : " + num.ToString();
    }
}
