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
    Explosion_bomb_small,
    BulletHit_normal,
    Explosion_bomb_large,
    Trail_Bullet,
    Sand_Scatter_Small,
    Sand_Scatter_Large,
    Explosion_CL,
    Explosion_EnergyBall,
    AttackSpark_EnergyBall,
    Projector_Explosion_Large,
    BulletHit_Trap,
}

public class Pool_Effect : MonoBehaviour
{
    [System.Serializable]
    struct EffectInfo
    {
        public GameObject prefab;
        public EffectType effectType;
        public int num;

        public EffectInfo(EffectInfo info)
        {
            this.prefab = Instantiate(info.prefab);
            this.prefab.transform.position = Vector3.up * 1000;
            this.effectType = info.effectType;
            num = 0;
        }
    }

    [SerializeField] private EffectInfo[] effectsInfo;
    private List<EffectInfo> effects = new List<EffectInfo>();

    // Start is called before the first frame update
    public void Init()
    {
        for (int i = 0; i < effectsInfo.Length; i++)
        {
            for (int j = 0; j < effectsInfo[i].num; j++)
            {
                EffectInfo temp = new EffectInfo(effectsInfo[i]);
         
                effects.Add(temp);
            }
        }

        Invoke("SetActiveFalseAll", 0.2f);
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
