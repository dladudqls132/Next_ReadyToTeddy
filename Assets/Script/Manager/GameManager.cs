using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private UI_CrosshairController UI_crosshairController;
    [SerializeField] private Pool_Enemy pool_enemy;
    [SerializeField] private Pool_Bullet pool_bullet;
    [SerializeField] private Pool_BulletHit pool_bulletHit;
    [SerializeField] private Pool_Effect pool_effect;
    [SerializeField] private ItemManager itemManager;
    [SerializeField] private Settings settings;
    [SerializeField] private UI_SettingController UI_settingController;
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private VideoController videoController;
    [SerializeField] private UI_GunSoundManager UI_gunSoundManager;
    [SerializeField] private UI_Fade UI_Fade;
    [SerializeField] private bool isPause;
    [SerializeField] private bool isGameOver;
    [SerializeField] private bool isCombat;
    [SerializeField] private bool isVisibleMousePoint;

    private static GameManager instance;

    public PlayerController GetPlayer() { return player; }
    public UI_CrosshairController GetCrosshairController() { return UI_crosshairController; }
    public Pool_Enemy GetPoolEnemy() { return pool_enemy; }
    public Pool_Bullet GetPoolBullet() { return pool_bullet; }
    public Pool_BulletHit GetPoolBulletHit() { return pool_bulletHit; }
    public Pool_Effect GetPoolEffect() { return pool_effect; }
    public ItemManager GetItemManager() { return itemManager; }
    public Settings GetSettings() { return settings; }
    public SoundManager GetSoundManager() { return soundManager; }
    public UI_SettingController GetSettingController() { return UI_settingController; }
    public VideoController GetVideoController() { return videoController; }
    public UI_GunSoundManager GetGunSoundManager() { return UI_gunSoundManager; }
    public UI_Fade GetFade() { return UI_Fade; }

    // Start is called before the first frame update
    void Awake()
    {
        Time.timeScale = 1;

        player = FindObjectOfType<PlayerController>();

        if (player != null)
            player.Init();

        UI_settingController = FindObjectOfType<UI_SettingController>();

        if (UI_settingController != null)
            UI_settingController.Init();

        UI_pause = FindObjectOfType<UI_Pause>();

        if (UI_pause != null)
            UI_pause.Init();

        UI_crosshairController = FindObjectOfType<UI_CrosshairController>();

        if (UI_crosshairController != null)
            UI_crosshairController.Init();

        pool_enemy = FindObjectOfType<Pool_Enemy>();

        if (pool_enemy != null)
            pool_enemy.Init();

        pool_bullet = FindObjectOfType<Pool_Bullet>();

        if (pool_bullet != null)
            pool_bullet.Init();

        pool_bulletHit = FindObjectOfType<Pool_BulletHit>();

        if (pool_bulletHit != null)
            pool_bulletHit.Init();

        pool_effect = FindObjectOfType<Pool_Effect>();

        if (pool_effect != null)
            pool_effect.Init();

        itemManager = FindObjectOfType<ItemManager>();

        if (itemManager != null)
            itemManager.Init();

        settings = FindObjectOfType<Settings>();

        if (settings != null)
            settings.Init();

        soundManager = FindObjectOfType<SoundManager>();

        if (soundManager != null)
            soundManager.Init();

        videoController = FindObjectOfType<VideoController>();

        if (videoController != null)
            videoController.Init();

        UI_gunSoundManager = FindObjectOfType<UI_GunSoundManager>();

        if (UI_gunSoundManager != null)
            UI_gunSoundManager.Init();

        UI_Fade = FindObjectOfType<UI_Fade>();

        if (isVisibleMousePoint)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    //private void Start()
    //{
    //    allAudioSources = FindObjectsOfType<AudioSource>();
    //}

    private void Update()
    {
        if(!isPause)
        {
            if (Time.timeScale == 0)
                return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
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
        if (player == null) return;


        if (videoController != null)
        {
            if (videoController.gameObject.activeSelf)
            {
                videoController.StopVideo();
                return;
            }
        }

        if (UI_pause != null)
        {
            if (!value)
            {
                if (UI_settingController.GetComponent<Animator>().GetBool("isOn"))
                {
                    UI_settingController.CancelInfo();
                    
                    return;
                }
            }

            if (value)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0.0f;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Time.timeScale = 1;
            }

            isPause = value;
            UI_pause.SetIsPause(value);
            soundManager.SetPauseAll(value);
        }
    }

    public void SetSlowMode(bool value)
    {
        if (isPause) return;

        if (value)
        {
            Time.timeScale = 0.05f;
        }
        else
        {
            Time.timeScale = 1;
        }

        soundManager.SetSlowAll(value);
    }

    public bool GetIsPause()
    {
        return isPause;
    }
}
