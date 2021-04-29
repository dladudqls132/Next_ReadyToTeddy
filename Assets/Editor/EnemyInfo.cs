using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "EnemyInfo", menuName = "Scriptable Object Asset/EnemyInfo")]
public class EnemyInfo : ScriptableObject
{
    [System.Serializable]
    public struct EnemyInfos
    {
        public GameObject prefab;
        public EnemyType enemyType;
        public CharacterMaterial material;
        public float hp;

        [Range(0, 100)]
        public float speed_min;
        [Range(0, 100)]
        public float speed_max;

        public float detectRange;
        public float attackRange;
    }

    public void SetInfo()
    {
        for(int i = 0; i < enemies.Length; i++)
        {
            Enemy temp = enemies[i].prefab.GetComponent<Enemy>();
            temp.SetInfo(enemies[i].enemyType, enemies[i].material, enemies[i].hp, enemies[i].speed_min, enemies[i].speed_max, enemies[i].detectRange, enemies[i].attackRange);
            EditorUtility.SetDirty(temp);
        }
    }

    public EnemyInfos[] enemies;
}
