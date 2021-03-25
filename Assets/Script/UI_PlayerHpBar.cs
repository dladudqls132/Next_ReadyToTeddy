using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerHpBar : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    private Image image;

    private void Start()
    {
        image = this.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.GetPlayer() != null)
        {
            if (GameManager.Instance.GetPlayer() != player)
            {
                player = GameManager.Instance.GetPlayer();
            }
        }

        image.rectTransform.sizeDelta = Vector2.Lerp(image.rectTransform.sizeDelta, new Vector2(800 * (player.GetCurrentHp() / player.GetMaxHp()), 80), Time.deltaTime * 10);
    }
}
