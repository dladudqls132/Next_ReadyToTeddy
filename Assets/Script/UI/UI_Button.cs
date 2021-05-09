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
        Mainmenu,
        GameStart,
        Credit,
        Question_On,
        Question_Off,
        Exit
    }

    [SerializeField] private ButtonType buttonType;
    [SerializeField] private GameObject image_on;
    [SerializeField] private GameObject image_off;
    [SerializeField] private GameObject question;
    [SerializeField] private string loadSceneName;

    public void SetActiveFalse()
    {
        image_off.SetActive(true);
        image_on.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image_on.SetActive(true);
        image_off.SetActive(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image_on.SetActive(false);
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
                    GameManager.Instance.LoadScene(GameManager.Instance.GetCurrentSceneIndex(), UnityEngine.SceneManagement.LoadSceneMode.Single);
                    break;
                case ButtonType.Settings:
                    break;
                case ButtonType.Mainmenu:
                    GameManager.Instance.LoadScene("MainMenu", UnityEngine.SceneManagement.LoadSceneMode.Single);
                    break;
                case ButtonType.GameStart:
                    GameManager.Instance.LoadScene(loadSceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
                    break;
                case ButtonType.Credit:
                    break;
                case ButtonType.Question_On:
                    question.SetActive(true);
                    break;
                case ButtonType.Question_Off:
                    SetActiveFalse();
                    question.SetActive(false);
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
