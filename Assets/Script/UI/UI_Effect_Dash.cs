using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Effect_Dash : MonoBehaviour
{
    private Image image;

    private void Start()
    {
        image = this.GetComponent<Image>();
    }
    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.GetPlayer().GetIsDash())
        {
            image.color = Color.Lerp(image.color, Color.white, Time.deltaTime * 15); 
        }
        else
        {
            image.color = Color.Lerp(image.color, new Color(1, 1, 1, (GameManager.Instance.GetPlayer().GetWalkSpeed() - GameManager.Instance.GetPlayer().GetWalkSpeed_Min()) / (GameManager.Instance.GetPlayer().GetWalkSpeed_Max() - GameManager.Instance.GetPlayer().GetWalkSpeed_Min())), Time.deltaTime * 25);
        }
    }
}
