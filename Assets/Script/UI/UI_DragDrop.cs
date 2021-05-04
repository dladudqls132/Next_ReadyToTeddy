using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_DragDrop : MonoBehaviour
{
    public int slotNum;
    public Sprite sprite;


    public void SetDragInfo(int slotNum, Sprite sprite)
    {
        this.slotNum = slotNum;
        this.sprite = sprite;

        this.GetComponent<Image>().sprite = sprite;
    }
}
