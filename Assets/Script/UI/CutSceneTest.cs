using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutSceneTest : MonoBehaviour
{
    [SerializeField] private int cutNum;
    [SerializeField] private GameObject[] images;
    [SerializeField] private GameObject nextCut;
    
    private bool isEnd;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            this.GetComponent<Animator>().SetTrigger("next");

            if (!isEnd)
            {
                if (cutNum >= images.Length - 1)
                {
                    NextStep();
                }
            }
        }
    }

    private void LateUpdate()
    {
        for(int i = 0; i < cutNum; i++)
        {
            images[i].SetActive(true);
        }
    }

    void OnDisplay()
    {
        cutNum++;
    }

    void NextStep()
    {
        isEnd = true;

        if (nextCut == null)
        {
            LoadingSceneController.LoadScene(CutSceneController.loadSceneName);
            return;
        }

        this.transform.parent.GetComponent<Animator>().SetTrigger("next");
        nextCut.SetActive(true);
    }
}
