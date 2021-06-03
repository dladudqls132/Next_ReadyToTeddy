using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_LoadScene : MonoBehaviour
{
    [SerializeField] private string loadSceneName;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GameManager.Instance.LoadScene(loadSceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
    }
}
