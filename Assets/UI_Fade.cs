using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum FadeState
{
    NoneFade,
    Fade,
    FadeOut,
    FadeIn
}

public class UI_Fade : MonoBehaviour
{
    [SerializeField] private FadeState state;
    private Image fadeImage;
    private float fadeSpeed = 4.5f;

    // Start is called before the first frame update
    void Start()
    {
        fadeImage = this.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case FadeState.NoneFade:
                fadeImage.color = Color.clear;
                break;
            case FadeState.Fade:
                fadeImage.color = Color.black;
                break;
            case FadeState.FadeIn:
                fadeImage.color = Color.Lerp(fadeImage.color, Color.clear, Time.unscaledDeltaTime * fadeSpeed);
                break;

            case FadeState.FadeOut:
                fadeImage.color = Color.Lerp(fadeImage.color, Color.black, Time.unscaledDeltaTime * fadeSpeed);
                break;
        }
    }

    public void SetFade(FadeState state, float fadeSpeed)
    {
        this.state = state;
        this.fadeSpeed = fadeSpeed;
    }
}
