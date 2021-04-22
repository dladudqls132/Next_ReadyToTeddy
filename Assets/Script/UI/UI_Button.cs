using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Button : MonoBehaviour
{
    enum ButtonType
    {
        Continue,
        Restart,
        Settings,
        Mainmenu
    }

    [SerializeField] private Camera _uiCamera;
    [SerializeField] private ButtonType buttonType;
    [SerializeField] private GameObject image_on;
    [SerializeField] private GameObject image_off;

    public void OnMouseEnter()
    {
        image_on.SetActive(true);
        image_off.SetActive(false);
    }

    public void OnMouseExit()
    {
        image_on.SetActive(false);
        image_off.SetActive(true);
    }

    public void OnMouseUp()
    {
        switch (buttonType)
        {
            case ButtonType.Continue:
                GameManager.Instance.SetIsPause(false);
                break;
            case ButtonType.Restart:
                GameManager.Instance.SetIsPause(false);
                GameManager.Instance.LoadScene(GameManager.Instance.GetCurrentSceneIndex(), UnityEngine.SceneManagement.LoadSceneMode.Single);
                break;
            case ButtonType.Settings:
                break;
            case ButtonType.Mainmenu:
                break;
        }
    }

    public void SetActiveFalse()
    {
        image_off.SetActive(true);
        image_on.SetActive(false);
    }
}
