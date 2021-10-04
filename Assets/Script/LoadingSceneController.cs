using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneController : MonoBehaviour
{
    private static string nextScene;
 
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadSceneProcess());
    }

    public static void LoadScene(string sceneName)
    {
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

            if(op.progress>=0.9f)
            {
                timer += Time.unscaledDeltaTime;

                if (timer >= 2.0f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }

        }
    }
}
