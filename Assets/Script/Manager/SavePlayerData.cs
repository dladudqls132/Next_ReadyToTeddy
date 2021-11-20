using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class PlayerData
{
    public List<GunType> gunType = new List<GunType>();
}

public class SavePlayerData : MonoBehaviour
{
    private string path;
    private PlayerData data;
    [SerializeField] private bool loadData;
    private bool isLoad;

    private void Start()
    {
        path = Application.dataPath + "/PlayerData.json";
    }

    private void Update()
    {
        if(!isLoad)
        {
            if (loadData)
            {
                isLoad = true;
                LoadData();
            }
        }
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
                    }
                }
            }
        }
    }

    public void SaveData()
    {
        data = new PlayerData();
        List<GameObject> guns = GameManager.Instance.GetPlayer().GetInventory().GetWeapons();

        for(int i = 0; i < guns.Count; i++)
        {
            data.gunType.Add(guns[i].GetComponent<Gun>().GetGunType());
        }
        
        File.WriteAllText(path, JsonUtility.ToJson(data));
    }
}
