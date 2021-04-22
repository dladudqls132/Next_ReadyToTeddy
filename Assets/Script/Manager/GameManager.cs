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
    [SerializeField] private Pool_Enemy pool_enemy;
    [SerializeField] private Pool_Bullet pool_bullet;
    [SerializeField] private bool isPause;
    [SerializeField] private bool isGameOver;
    [SerializeField] private bool isCombat;

    private static GameManager instance;

    public PlayerController GetPlayer() { return player; }
    public UI_Crosshair GetCrosshair() { return UI_crosshair; }
    public Pool_Enemy GetPoolEnemy() { return pool_enemy; }
    public Pool_Bullet GetPoolBullet() { return pool_bullet; }

    // Start is called before the first frame update
    void Awake()
    {
        player = FindObjectOfType<PlayerController>();

        if(player != null)
            player.Init();

        UI_pause = FindObjectOfType<UI_Pause>();

        if(UI_pause != null)
            UI_pause.Init();

        UI_crosshair = FindObjectOfType<UI_Crosshair>();

        if(UI_crosshair != null)
            UI_crosshair.Init();

        pool_enemy = FindObjectOfType<Pool_Enemy>();

        if(pool_enemy != null)
            pool_enemy.Init();

        pool_bullet = FindObjectOfType<Pool_Bullet>();

        if (pool_bullet != null)
            pool_bullet.Init();
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

    public void SetIsGameOver(bool value) { isGameOver = value; }
    public bool GetIsGameOver() { return isGameOver; }

    public void SetIsCombat(bool value) { isCombat = value; }
    public bool GetIsCombat() { return isCombat; }

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
