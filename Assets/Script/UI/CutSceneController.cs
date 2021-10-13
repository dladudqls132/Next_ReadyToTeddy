using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneController : MonoBehaviour
{
    [SerializeField] private string m_loadSceneName;
    public static string loadSceneName;
    public static bool isDisplay;

    private void Start()
    {
        isDisplay = true;
        loadSceneName = m_loadSceneName;
    }
}
