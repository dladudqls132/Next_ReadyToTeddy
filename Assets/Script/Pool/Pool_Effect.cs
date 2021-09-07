using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    None,
    Damaged_normal,
    Damaged_lightning,
    AttackSpark_normal,
    Explosion_destroy,
    Explosion_bomb,
    BulletHit_normal,
}

public class Pool_Effect : MonoBehaviour
{
    [System.Serializable]
    struct EffectInfo
    {
        public GameObject prefab;
        public EffectType effectType;

        public EffectInfo(EffectInfo info)
        {
            this.prefab = Instantiate(info.prefab);
            //this.prefab.SetActive(false);
            this.prefab.transform.position = Vector3.up * 1000;
            this.effectType = info.effectType;
        }
    }

    [SerializeField] private int effectNum = 0;
    [SerializeField] private EffectInfo[] effectsInfo;
    private List<EffectInfo> effects = new List<EffectInfo>();

    // Start is called before the first frame update
    public void Init()
    {
        for (int i = 0; i < effectsInfo.Length; i++)
        {
            for (int j = 0; j < effectNum; j++)
            {
                EffectInfo temp = new EffectInfo(effectsInfo[i]);
         
                effects.Add(temp);
            }
        }

        Invoke("SetActiveFalseAll", 1f);
    }

    void SetActiveFalseAll()
    {
        for (int i = 0; i < effects.Count; i++)
        {

            effects[i].prefab.SetActive(false);
        }
    }

    private void Update()
    {
        for (int i = 0; i < effects.Count; i++)
        {
            if (!effects[i].prefab.activeSelf)
                effects[i].prefab.transform.SetParent(this.transform);
        }
    }

    public GameObject GetEffect(EffectType effectType)
    {
        for (int i = 0; i < effects.Count; i++)
        {
            if (effects[i].effectType == effectType)
            {
                if (!effects[i].prefab.activeSelf)
                    return effects[i].prefab;
            }
        }

        return null;
    }
}
