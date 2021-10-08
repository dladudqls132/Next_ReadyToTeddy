using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneController : MonoBehaviour
{
    [SerializeField] private Image progressbar;
    private static string nextScene;
    [SerializeField] private Text text_background;
    [SerializeField] private Text text;
    [SerializeField] private GameObject rascal;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadSceneProcess());
    }

    public static void LoadScene(string sceneName)
    {
        if (sceneName.Equals("CutScene_Start"))
        {
            if (CutSceneController.isDisplay)
            {
                sceneName = CutSceneController.loadSceneName;
            }
        }

        nextScene = sceneName;

        SceneManager.LoadScene("LoadingScene");
    }

    public static void ReloadScene()
    {
        nextScene = SceneManager.GetActiveScene().name;

        SceneManager.LoadScene("LoadingScene");
    }

    IEnumerator LoadSceneProcess()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;
        float timer = 0;

        while(!op.isDone)
        {
            yield return null;

            //if (op.progress < 0.9f)
            //{
            //    progressbar.fillAmount = op.progress;
            //}
            //else
            //{
            //    timer += Time.unscaledDeltaTime;
            //    progressbar.fillAmount = Mathf.Lerp(0.9f, 1f, timer);

            //    if (timer >= 2.0f)
            //    {
            //        op.allowSceneActivation = true;
            //        yield break;
            //    }
            //}

            if(progressbar.fillAmount < 0.9f)
            {
                progressbar.fillAmount = Mathf.Lerp(progressbar.fillAmount, 1.0f, Time.unscaledDeltaTime / 2);

                text_background.text = (progressbar.fillAmount * 100).ToString("N1") + " %";
                text.text = (progressbar.fillAmount * 100).ToString("N1") + " %";

                rascal.transform.localPosition = new Vector3(-912.0f + (1824 * (progressbar.fillAmount)), -418.7f, 0f);
                rascal.GetComponent<Animator>().SetFloat("speed", (0.9f - progressbar.fillAmount) * 1.2f);
            }
            else
            {
                progressbar.fillAmount = Mathf.MoveTowards(progressbar.fillAmount, 1.0f, Time.unscaledDeltaTime / 5);

                text_background.text = (progressbar.fillAmount * 100).ToString("N1") + " %";
                text.text = (progressbar.fillAmount * 100).ToString("N1") + " %";
                
                rascal.transform.localPosition = new Vector3(-912.0f + (1824 * (progressbar.fillAmount)), -418.7f, 0f);
                rascal.GetComponent<Animator>().SetFloat("speed", 1.0f);

                if (progressbar.fillAmount >= 0.99f)
                {
                    timer += Time.unscaledDeltaTime;

                    if(timer >= 1.0f)
                    {
                        op.allowSceneActivation = true;
                        yield break;
                    }
                }
            }
        }
    }
}
