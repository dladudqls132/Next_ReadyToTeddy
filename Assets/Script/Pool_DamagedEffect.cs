using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool_DamagedEffect : MonoBehaviour
{
    public enum Material
    {
        None,
        Iron
    }

    [System.Serializable]
    struct EffectInfo
    {
        public GameObject prefab;
        public Material material;

        public EffectInfo(EffectInfo info)
        {
            this = info;
        }

        public EffectInfo(GameObject prefab, Material material)
        {
            this.prefab = prefab;
            this.material = material;
        }
    }

    [SerializeField] private EffectInfo[] damagedEffectsInfo;
    [SerializeField] private List<EffectInfo> damagedEffects = new List<EffectInfo>();
    [SerializeField] private int effectNum;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < damagedEffectsInfo.Length; i++)
        {
            for (int j = 0; j < effectNum; j++)
            {
                GameObject temp = Instantiate(damagedEffectsInfo[i].prefab, this.transform);
                damagedEffects.Add(damagedEffectsInfo[i]);
                temp.SetActive(false);
            }
        }
    }

    private void Update()
    {
        for (int i = 0; i < damagedEffects.Count; i++)
        {
            if (!damagedEffects[i].prefab.activeSelf && damagedEffects[i].prefab.transform.parent == null)
                damagedEffects[i].prefab.transform.SetParent(this.transform);
        }
    }

    public GameObject GetDamagedEffect(Material material)
    {
        for(int i = 0; i < damagedEffects.Count; i++)
        {
            if (damagedEffects[i].material == material)
            {
                if (!damagedEffects[i].prefab.activeSelf)
                    return damagedEffects[i].prefab;
            }
        }

        return null;
    }
}
