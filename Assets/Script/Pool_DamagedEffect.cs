using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool_DamagedEffect : MonoBehaviour
{
    [SerializeField] private GameObject damagedEffectPrefab;
    [SerializeField] private List<GameObject> damagedEffects = new List<GameObject>();
    [SerializeField] private int effectNum;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < effectNum; i++)
        {
            damagedEffects.Add(Instantiate(damagedEffectPrefab, this.transform));
            damagedEffects[i].SetActive(false);
        }
    }

    private void Update()
    {
        for (int i = 0; i < damagedEffects.Count; i++)
        {
            if (!damagedEffects[i].activeSelf && damagedEffects[i].transform.parent == null)
                damagedEffects[i].transform.SetParent(this.transform);
        }
    }

    public GameObject GetDamagedEffect()
    {
        for(int i = 0; i < damagedEffects.Count; i++)
        {
            if (!damagedEffects[i].activeSelf)
                return damagedEffects[i];
        }

        return null;
    }
}
