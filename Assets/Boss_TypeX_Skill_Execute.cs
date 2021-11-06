using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_TypeX_Skill_Execute : Boss_Skill
{
    [SerializeField] private Transform center;
    [SerializeField] private GameObject prefab_executeBall;
    [SerializeField] private GameObject barrier;
    private List<GameObject> executeBalls = new List<GameObject>();
    [SerializeField] private int executeBallNum;
    [SerializeField] private int eattenBallNum;
    [SerializeField] private int useNum;

    protected override void Awake()
    {
        base.Awake();

        for (int i = 0; i < executeBallNum; i++)
        {
            GameObject temp = Instantiate(prefab_executeBall);
            executeBalls.Add(temp);
            temp.SetActive(false);
        }
    }

    protected override void Start()
    {
        base.Start();
    }

    public void IncreaseEattenBallNum()
    {
        eattenBallNum++;
    }

    public override void Use()
    {
        base.Use();

        barrier.SetActive(true);
        anim.SetBool("isExecute", true);

        GameManager.Instance.GetPlayer().GetCam().Shake(100, 0.15f, true);

        for(int i = 0; i < executeBalls.Count; i++)
        {
            //executeBalls[i].transform.position = center.position + Quaternion.Euler(0, (360 / executeBalls.Count) * i, 0) * this.transform.forward * Random.Range(5, 10);
            executeBalls[i].GetComponent<Boss_TypeX_Skill_ExecuteBall>().ActiveTrue(center.position + Quaternion.Euler(0, (360 / executeBalls.Count) * i, 0) * this.transform.forward * Random.Range(25, 35), center.transform, 2);
        }
        //for (int i = 0; i < num; i++)
        //{
        //    GameObject tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Plasma);
        //    tempBullet.GetComponent<Bullet>().SetFire(center.position + Random.insideUnitSphere * 4, (target.position - this.transform.position).normalized, target, speed, damage, 999);
        //}
    }

    protected override void ResetInfo()
    {
        anim.SetBool("isExecute", false);

        base.ResetInfo();
    }

    protected override void Update()
    {
        bool isEnd = true;
        for (int i = 0; i < executeBalls.Count; i++)
        {
            if (executeBalls[i].activeSelf) 
                isEnd = false;
        }
        if (isEnd)
        {
            if (eattenBallNum >= useNum)
            {
                if (barrier.activeSelf)
                {
                    GameManager.Instance.GetPlayer().GetCam().Shake(0, 0, true);

                    barrier.SetActive(false);
                }
                Debug.LogError("use");
            }
            else
            {
                if (barrier.activeSelf)
                {
                    GameManager.Instance.GetPlayer().GetCam().Shake(0, 0, true);
                    barrier.SetActive(false);

                    anim.SetBool("isExecute", false);
                    anim.SetBool("isDefeat", true);
                    this.GetComponent<Boss_TypeX>().SetPhase(5);
                }
            }
        }
        //currentAttackTime -= Time.deltaTime;

        //anim.SetBool("isAttack_EnergyBall_LeftHand", true);

        //if (currentAttackTime <= 0)
        //{
        //    ResetInfo();
        //}
    }
}
