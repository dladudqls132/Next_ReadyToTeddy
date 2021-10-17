using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

    [SerializeField] private bool isAnim;
    [SerializeField] private bool isOn;
    private bool canMove;
    private Vector3 originPos_image_On;
    private Vector3 originPos_image_Off;

    void Start()
    {
        if (isAnim)
        {
            if (image_on != null && image_off != null)
            {
                originPos_image_On = image_on.transform.localPosition;
                originPos_image_Off = image_off.transform.localPosition;
            }
        }
    }

    IEnumerator SetCanMove()
    {

        yield return new WaitForSecondsRealtime(0.2f);

        canMove = true;
    }

    void OnEnable()
    {
        StartCoroutine(SetCanMove());
    }

    void OnDisable()
    {
        if (canMove)
        {
            isOn = false;
            canMove = false;

            if (image_off != null && image_on != null)
            {
                image_off.transform.localPosition = originPos_image_Off;
                image_on.transform.localPosition = originPos_image_On;
            }
        }
    }


    void Update()
    {
        if (isAnim && canMove)
        {
            if (isOn)
            {
                image_on.transform.localPosition = Vector3.Lerp(image_on.transform.localPosition, originPos_image_On + Vector3.right * 50, Time.unscaledDeltaTime * 10);
                image_off.transform.localPosition = Vector3.Lerp(image_off.transform.localPosition, originPos_image_Off + Vector3.right * 50, Time.unscaledDeltaTime * 10);
            }
            else
            {
                image_on.transform.localPosition = Vector3.Lerp(image_on.transform.localPosition, originPos_image_On, Time.unscaledDeltaTime * 10);
                image_off.transform.localPosition = Vector3.Lerp(image_off.transform.localPosition, originPos_image_Off, Time.unscaledDeltaTime * 10);
            }
        }
    }

    public void SetActiveFalse()
    {
        if (image_off != null)
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

        if (isAnim)
            isOn = true;

        if (Input.GetMouseButton(0))
        {
            image_on.GetComponent<Image>().color = Color.gray;
        }
        else
        {
            image_on.GetComponent<Image>().color = Color.white;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (image_on != null)
            image_on.SetActive(false);
        if (image_off != null)
            image_off.SetActive(true);

        if (isAnim)
            isOn = false;

        image_on.GetComponent<Image>().color = Color.white;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.pointerEnter != null)
            if (eventData.pointerEnter.GetComponent<UI_Button>() != null)
            {
                switch (eventData.pointerEnter.GetComponent<UI_Button>().buttonType)
                {
                    case ButtonType.Continue:
                        GameManager.Instance.SetIsPause(false);
                        break;
                    case ButtonType.Restart:
                        GameManager.Instance.SetIsPause(false);
                        LoadingSceneController.ReloadScene();

                        break;
                    case ButtonType.Settings:
                        GameManager.Instance.GetSettingController().ToggleMenu();
                        eventData.pointerEnter.GetComponent<UI_Button>().setting.SetActive(true);
                        break;
                    case ButtonType.Setting_Apply:
                        GameManager.Instance.GetSettingController().ApplyInfo();
                        eventData.pointerEnter.GetComponent<UI_Button>().SetActiveFalse();
                        SetActiveFalse();
                        //setting.SetActive(false);
                        break;
                    case ButtonType.Setting_Cancel:
                        GameManager.Instance.GetSettingController().CancelInfo();
                        eventData.pointerEnter.GetComponent<UI_Button>().SetActiveFalse();
                        SetActiveFalse();
                        //setting.SetActive(false);
                        break;
                    case ButtonType.Question_On:
                        eventData.pointerEnter.GetComponent<UI_Button>().question.SetActive(false);
                        SetActiveFalse();
                        question.SetActive(true);
                        break;
                    case ButtonType.Question_Off:
                        eventData.pointerEnter.GetComponent<UI_Button>().question.SetActive(false);
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

                image_on.GetComponent<Image>().color = Color.white;
            }

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        image_on.GetComponent<Image>().color = Color.gray;
    }
}
