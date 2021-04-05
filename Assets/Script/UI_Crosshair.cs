using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Crosshair : MonoBehaviour
{
    private PlayerController player;
    private RectTransform rectTransform;
    private Vector3 originScale;

    private void Start()
    {
        rectTransform = this.GetComponent<RectTransform>();
        originScale = rectTransform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null && GameManager.Instance.GetPlayer() != null)
            player = GameManager.Instance.GetPlayer();

        if(player.GetIsAiming())
        {
            rectTransform.localScale = Vector3.Lerp(rectTransform.localScale, originScale / 1.5f, Time.deltaTime * 12);
        }
        else
        {
            rectTransform.localScale = Vector3.Lerp(rectTransform.localScale, originScale, Time.deltaTime * 12);
        }
    }
}
