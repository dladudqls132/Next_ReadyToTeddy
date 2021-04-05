using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterMaterial
{
    None,
    Iron
}

public class Pool_DamagedEffect : MonoBehaviour
{
    [System.Serializable]
    struct EffectInfo
    {
        public GameObject prefab;
        public CharacterMaterial material;

        public EffectInfo(EffectInfo info)
        {
            this.prefab = Instantiate(info.prefab);
            this.prefab.SetActive(false);
            this.material = info.material;
        }
    }

    [SerializeField] private int effectNum = 0;
    [SerializeField] private EffectInfo[] damagedEffectsInfo;
    private List<EffectInfo> damagedEffects = new List<EffectInfo>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < damagedEffectsInfo.Length; i++)
        {
            for (int j = 0; j < effectNum; j++)
            {
                EffectInfo temp = new EffectInfo(damagedEffectsInfo[i]);
                damagedEffects.Add(temp);
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

    public GameObject GetDamagedEffect(CharacterMaterial material)
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
