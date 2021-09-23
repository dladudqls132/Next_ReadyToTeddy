using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    enum ButtonType
    {
        Continue,
        Restart,
        Settings,
        Question_On,
        Question_Off,
        LoadScene,
        Exit,
        Setting_Apply,
        Setting_Cancel
    }

    [SerializeField] private ButtonType buttonType;
    [SerializeField] private GameObject image_on;
    [SerializeField] private GameObject image_off;
    [SerializeField] private GameObject question;
    [SerializeField] private GameObject setting;
    [SerializeField] private string loadSceneName;

    public void SetActiveFalse()
    {
        if(image_off != null)
            image_off.SetActive(true);
        if (image_on != null)
            image_on.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (image_on != null)
            image_on.SetActive(true);
        if (image_off != null)
            image_off.SetActive(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (image_on != null)
            image_on.SetActive(false);
        if (image_off != null)
            image_off.SetActive(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.pointerEnter.GetComponent<UI_Button>() != null)
        {
            switch (eventData.pointerEnter.GetComponent<UI_Button>().buttonType)
            {
                case ButtonType.Continue:
                    GameManager.Instance.SetIsPause(false);
                    break;
                case ButtonType.Restart:
                    GameManager.Instance.SetIsPause(false);
                    //GameManager.LoadScene(GameManager.Instance.GetCurrentSceneIndex());
                    
                    break;
                case ButtonType.Settings:
                    setting.SetActive(true);
                    break;
                case ButtonType.Setting_Apply:
                    //GameManager.Instance.GetSettings().SaveData();
                    GameManager.Instance.GetSettingController().ApplyInfo();
                    SetActiveFalse();
                    setting.SetActive(false);
                    break;
                case ButtonType.Setting_Cancel:
                    GameManager.Instance.GetSettingController().CancelInfo();
                    SetActiveFalse();
                    setting.SetActive(false);
                    break;
                case ButtonType.Question_On:
                    question.SetActive(true);
                    break;
                case ButtonType.Question_Off:
                    SetActiveFalse();
                    question.SetActive(false);
                    break;
                case ButtonType.LoadScene:
                    LoadingSceneController.LoadScene(loadSceneName);
                    break;
                case ButtonType.Exit:
                    Application.Quit();
                    break;
            }
        }

    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }
}
