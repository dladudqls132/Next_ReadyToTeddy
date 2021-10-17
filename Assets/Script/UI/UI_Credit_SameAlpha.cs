using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Credit_SameAlpha : MonoBehaviour
{
    Image image;
    Image parentImage;

    // Start is called before the first frame update
    void Start()
    {
        image = this.GetComponent<Image>();
        parentImage = this.transform.parent.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        image.color = parentImage.color;
    }
}
