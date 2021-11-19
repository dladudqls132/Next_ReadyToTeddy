using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CreditController : MonoBehaviour
{
    private Animator anim;
    private float originAnimSpeed;

    // Start is called before the first frame update
    void Start()
    {
        anim = this.GetComponent<Animator>();
        originAnimSpeed = anim.speed;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.KeypadEnter))
        {
            anim.speed = originAnimSpeed * 4;
        }
        else
        {
            anim.speed = originAnimSpeed;
        }
    }

    void LoadMainMenu()
    {
        LoadingSceneController.LoadSceneSkipLoading("MainMenu");
    }
}
