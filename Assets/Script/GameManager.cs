using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(GameManager)) as GameManager;
            }

            return instance;
        }
        set { instance = value; }
    }

    [SerializeField] private PlayerController player;
    [SerializeField] private UI_Pause UI_pause;
    [SerializeField] private UI_Crosshair UI_crosshair;
    [SerializeField] private bool isPause;

    private static GameManager instance;

    public PlayerController GetPlayer() { return player; }
    public UI_Crosshair GetCrosshair() { return UI_crosshair; }

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        player.Init();

        UI_pause = FindObjectOfType<UI_Pause>();
        UI_pause.Init();

        UI_crosshair = FindObjectOfType<UI_Crosshair>();
        UI_crosshair.Init();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SetIsPause(!isPause);
        }
    }

    private void OnDestroy()
    {
        instance = null;
    }

    public void SetIsPause(bool value)
    {
        UI_pause.SetIsPause(value);

        isPause = value;
    }

    public bool GetIsPause()
    {
        return isPause;
    }

    public int GetCurrentSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    public void LoadScene(string name, LoadSceneMode mode)
    {
        SceneManager.LoadScene(name, mode);
    }

    public void LoadScene(int index, LoadSceneMode mode)
    {
        SceneManager.LoadScene(index, mode);
    }
}
