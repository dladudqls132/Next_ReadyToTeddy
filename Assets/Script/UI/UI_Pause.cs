using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Pause : MonoBehaviour
{
    //[SerializeField] private bool isPause = false;
    [SerializeField] private GameObject root;
    [SerializeField] private UI_Button[] buttons;
    [SerializeField] private Camera cam_UI;
    [SerializeField] private UI_Button currentButton;

    public void Init()
    {
        buttons = root.GetComponentsInChildren<UI_Button>();

        foreach (UI_Button child in buttons)
        {
            child.SetActiveFalse();
        }
    }

    private void Update()
    {
        var ray = cam_UI.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("UI"));

        if(hit.transform != null)
        {
            currentButton = hit.transform.GetComponent<UI_Button>();
            currentButton.OnMouseEnter();
        }
        else
        {
            if (currentButton != null)
            {
                currentButton.OnMouseExit();
                currentButton = null;
            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            if(currentButton != null)
                currentButton.OnMouseUp();
        }
    }

    public void SetIsPause(bool value)
    {
        if(value)
        {
            root.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
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
