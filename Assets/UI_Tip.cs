using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Tip : MonoBehaviour
{
    [TextArea]
    [SerializeField] private string[] texts;
    [SerializeField] private float changeTime;
    [SerializeField] private int num;
    [SerializeField] private Text text;

    // Start is called before the first frame update
    void Start()
    {
        num = Random.Range(0, texts.Length);

        text.text = texts[num];
        num++;
        num %= texts.Length;

        StartCoroutine(ChangeText());
    }

    IEnumerator ChangeText()
    {
        while (true)
        {
            yield return new WaitForSeconds(changeTime);
            text.text = texts[num];
            num++;
            num %= texts.Length;
        }

    }
}
