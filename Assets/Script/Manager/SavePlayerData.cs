using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class PlayerData
{
    public List<GunType> gunType = new List<GunType>();
    public List<int> currentAmmo = new List<int>();
    public List<int> haveAmmo = new List<int>();
    public float hp;
    public int stageNum;
}

public class SavePlayerData : MonoBehaviour
{
    private string path;
    private PlayerData data;
    [SerializeField] private bool loadData;
    [SerializeField] private bool resetInfo;
    private bool isLoad;

    public void Init()
    {
        path = Application.dataPath + "/PlayerData.json";

        if (resetInfo)
        {
            if (File.Exists(path))
            {
                ResetInfo();
            }
        }
    }

    private void Update()
    {
        //if(!isLoad)
        //{
        //    if (loadData)
        //    {
        //        isLoad = true;
        //        LoadData();
        //    }
        //}
    }

    public void LoadData()
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            data = JsonUtility.FromJson<PlayerData>(json);

            for(int i = 1; i < data.gunType.Count; i++)
            {
                for(int j = 0; j < GameManager.Instance.GetPlayer().GetWeapons().Length; j++)
                {
                    if (GameManager.Instance.GetPlayer().GetWeapons()[j].GetComponent<Gun>().GetGunType() == data.gunType[i])
                    {
                        GameObject temp = Instantiate(GameManager.Instance.GetPlayer().GetWeapons()[j]);
                        GameManager.Instance.GetPlayer().GetInventory().AddWeaponSkipVideo(temp);
                        temp.GetComponent<Gun>().SetCurrentAmmo(data.currentAmmo[i]);
                        temp.GetComponent<Gun>().SetHaveAmmo(data.haveAmmo[i]);
                        break;
                    }
                }
            }

            GameManager.Instance.GetPlayer().SetCurrentHp(data.hp);
            GameManager.Instance.GetPlayer().SetSaveStageNum(data.stageNum);
        }
    }

    //public void SaveData()
    //{
    //    data = new PlayerData();
    //    List<GameObject> guns = GameManager.Instance.GetPlayer().GetInventory().GetWeapons();

    //    for(int i = 0; i < guns.Count; i++)
    //    {
    //        data.gunType.Add(guns[i].GetComponent<Gun>().GetGunType());
    //    }

    //    File.WriteAllText(path, JsonUtility.ToJson(data));
    //}

    public void SaveData(int stageNum)
    {
        data = new PlayerData();
        List<GameObject> guns = GameManager.Instance.GetPlayer().GetInventory().GetWeapons();

        for (int i = 0; i < guns.Count; i++)
        {
            data.gunType.Add(guns[i].GetComponent<Gun>().GetGunType());
            data.currentAmmo.Add(guns[i].GetComponent<Gun>().GetCurrentAmmoCount());
            data.haveAmmo.Add(guns[i].GetComponent<Gun>().GetHaveAmmoCount());
        }

        data.hp = GameManager.Instance.GetPlayer().GetCurrentHp();
        data.stageNum = stageNum;

        File.WriteAllText(path, JsonUtility.ToJson(data));
    }

    public void ResetInfo()
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    private void OnApplicationQuit()
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}
