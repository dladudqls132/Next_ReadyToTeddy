using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Pause : MonoBehaviour
{
    //[SerializeField] private bool isPause = false;
    [SerializeField] private GameObject root;
    [SerializeField] private UI_Button[] buttons;
    
    public GameObject GetRoot() { return root; }

    public void Init()
    {
        buttons = root.GetComponentsInChildren<UI_Button>();

        foreach (UI_Button child in buttons)
        {
            child.SetActiveFalse();
        }

        this.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void SetIsPause(bool value)
    {
        if(value)
        {
            root.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0.05f;
        }
        else
        {
            foreach (UI_Button child in buttons)
            {
                child.SetActiveFalse();
            }

            root.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1;
        }
    }
}
