using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_TypeX_Skill_RandomShot : Boss_Skill
{
    [SerializeField] private Transform firePos_bottom;
    [SerializeField] private Transform firePos_middle;
    [SerializeField] private Transform firePos_top;
    [SerializeField] private float speed;
    [SerializeField] private float attackTime;
    private float currentAttackTime;

    [SerializeField] private float delay;
    private float currentDelay;

    List<int> rndNum = new List<int>();
  

    protected override void Start()
    {
        base.Start();

        for (int i = 0; i < 6; i++)
            rndNum.Add(i);
    }

    public override void Use()
    {
        base.Use();
        currentAttackTime = attackTime;
        currentDelay = delay;

        StartCoroutine(AttackRnd());
        anim.SetBool("isRandomShot", true);
    }

    protected override void ResetInfo()
    {
        anim.SetBool("isRandomShot", false);

        base.ResetInfo();
    }

    void Fire_Bottom_Horizontal()
    {
        int tempRnd = Random.Range(0, 2);

        if (tempRnd == 0)
        {
            Bullet_Boss tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
            tempBullet.Fire(firePos_bottom.position, firePos_bottom.forward, speed, damage);

            tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
            tempBullet.Fire(firePos_bottom.position, Quaternion.Euler(0, 15, 0) * firePos_bottom.forward, speed, damage);

            tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
            tempBullet.Fire(firePos_bottom.position, Quaternion.Euler(0, -15, 0) * firePos_bottom.forward, speed, damage);

            tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
            tempBullet.Fire(firePos_bottom.position, Quaternion.Euler(0, 30, 0) * firePos_bottom.forward, speed, damage);

            tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
            tempBullet.Fire(firePos_bottom.position, Quaternion.Euler(0, -30, 0) * firePos_bottom.forward, speed, damage);
        }
        else
        {
            Bullet_Boss tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
            tempBullet.Fire(firePos_bottom.position, firePos_bottom.forward, speed, damage);

            tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
            tempBullet.Fire(firePos_bottom.position, Quaternion.Euler(0, 7.5f, 0) * firePos_bottom.forward, speed, damage);

            tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
            tempBullet.Fire(firePos_bottom.position, Quaternion.Euler(0, -7.5f, 0) * firePos_bottom.forward, speed, damage);

            tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
            tempBullet.Fire(firePos_bottom.position, Quaternion.Euler(0, 22.5f, 0) * firePos_bottom.forward, speed, damage);

            tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
            tempBullet.Fire(firePos_bottom.position, Quaternion.Euler(0, -22.5f, 0) * firePos_bottom.forward, speed, damage);
        }
    }
    
    void Fire_Middle_Horizontal()
    {
        int tempRnd = Random.Range(0, 2);

        if (tempRnd == 0)
        {
            Bullet_Boss tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
            tempBullet.Fire(firePos_middle.position, firePos_middle.forward, speed, damage);

            tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
            tempBullet.Fire(firePos_middle.position, Quaternion.Euler(0, 15, 0) * firePos_middle.forward, speed, damage);

            tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
            tempBullet.Fire(firePos_middle.position, Quaternion.Euler(0, -15, 0) * firePos_middle.forward, speed, damage);

            tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
            tempBullet.Fire(firePos_middle.position, Quaternion.Euler(0, 30, 0) * firePos_middle.forward, speed, damage);

            tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
            tempBullet.Fire(firePos_middle.position, Quaternion.Euler(0, -30, 0) * firePos_middle.forward, speed, damage);
        }
        else
        {
            Bullet_Boss tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
            tempBullet.Fire(firePos_middle.position, firePos_middle.forward, speed, damage);

            tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
            tempBullet.Fire(firePos_middle.position, Quaternion.Euler(0, 7.5f, 0) * firePos_middle.forward, speed, damage);

            tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
            tempBullet.Fire(firePos_middle.position, Quaternion.Euler(0, -7.5f, 0) * firePos_middle.forward, speed, damage);

            tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
            tempBullet.Fire(firePos_middle.position, Quaternion.Euler(0, 22.5f, 0) * firePos_middle.forward, speed, damage);

            tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
            tempBullet.Fire(firePos_middle.position, Quaternion.Euler(0, -22.5f, 0) * firePos_middle.forward, speed, damage);
        }
    }

    void Fire_Top_Horizontal()
    {
        int tempRnd = Random.Range(0, 2);

        if (tempRnd == 0)
        {
            Bullet_Boss tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
            tempBullet.Fire(firePos_top.position, firePos_top.forward, speed, damage);

            tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
            tempBullet.Fire(firePos_top.position, Quaternion.Euler(0, 15, 0) * firePos_top.forward, speed, damage);

            tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
            tempBullet.Fire(firePos_top.position, Quaternion.Euler(0, -15, 0) * firePos_top.forward, speed, damage);

            tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
            tempBullet.Fire(firePos_top.position, Quaternion.Euler(0, 30, 0) * firePos_top.forward, speed, damage);

            tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
            tempBullet.Fire(firePos_top.position, Quaternion.Euler(0, -30, 0) * firePos_top.forward, speed, damage);
        }
        else
        {
            Bullet_Boss tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
            tempBullet.Fire(firePos_top.position, firePos_top.forward, speed, damage);

            tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
            tempBullet.Fire(firePos_top.position, Quaternion.Euler(0, 7.5f, 0) * firePos_top.forward, speed, damage);

            tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
            tempBullet.Fire(firePos_top.position, Quaternion.Euler(0, -7.5f, 0) * firePos_top.forward, speed, damage);

            tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
            tempBullet.Fire(firePos_top.position, Quaternion.Euler(0, 22.5f, 0) * firePos_top.forward, speed, damage);

            tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
            tempBullet.Fire(firePos_top.position, Quaternion.Euler(0, -22.5f, 0) * firePos_top.forward, speed, damage);
        }
    }

    void Fire_Middle_Vertical()
    {
        Bullet_Boss tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
        tempBullet.Fire(firePos_bottom.position, firePos_bottom.forward, speed, damage);

        tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
        tempBullet.Fire(firePos_middle.position, firePos_middle.forward, speed, damage);

        tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
        tempBullet.Fire(firePos_top.position, firePos_top.forward, speed, damage);

        tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
        tempBullet.Fire(firePos_bottom.position, Quaternion.Euler(0, -15, 0) * firePos_bottom.forward, speed, damage);

        tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
        tempBullet.Fire(firePos_middle.position, Quaternion.Euler(0, -15, 0) * firePos_middle.forward, speed, damage);

        tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
        tempBullet.Fire(firePos_top.position, Quaternion.Euler(0, -15, 0) * firePos_top.forward, speed, damage);

        tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
        tempBullet.Fire(firePos_bottom.position, Quaternion.Euler(0, 15, 0) * firePos_bottom.forward, speed, damage);

        tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
        tempBullet.Fire(firePos_middle.position, Quaternion.Euler(0, 15, 0) * firePos_middle.forward, speed, damage);

        tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
        tempBullet.Fire(firePos_top.position, Quaternion.Euler(0, 15, 0) * firePos_top.forward, speed, damage);
    }

    void Fire_Left_Vertical()
    {
        Bullet_Boss tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
        tempBullet.Fire(firePos_bottom.position, Quaternion.Euler(0, 30, 0) * firePos_bottom.forward, speed, damage);

        tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
        tempBullet.Fire(firePos_middle.position, Quaternion.Euler(0, 30, 0) * firePos_middle.forward, speed, damage);

        tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
        tempBullet.Fire(firePos_top.position, Quaternion.Euler(0, 30, 0) * firePos_top.forward, speed, damage);

        tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
        tempBullet.Fire(firePos_bottom.position, Quaternion.Euler(0, 15, 0) * firePos_bottom.forward, speed, damage);

        tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
        tempBullet.Fire(firePos_middle.position, Quaternion.Euler(0, 15, 0) * firePos_middle.forward, speed, damage);

        tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
        tempBullet.Fire(firePos_top.position, Quaternion.Euler(0, 15, 0) * firePos_top.forward, speed, damage);
    }

    void Fire_Right_Vertical()
    {
        Bullet_Boss tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
        tempBullet.Fire(firePos_bottom.position, Quaternion.Euler(0, -30, 0) * firePos_bottom.forward, speed, damage);

        tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
        tempBullet.Fire(firePos_middle.position, Quaternion.Euler(0, -30, 0) * firePos_middle.forward, speed, damage);

        tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
        tempBullet.Fire(firePos_top.position, Quaternion.Euler(0, -30, 0) * firePos_top.forward, speed, damage);

        tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
        tempBullet.Fire(firePos_bottom.position, Quaternion.Euler(0, -15, 0) * firePos_bottom.forward, speed, damage);

        tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
        tempBullet.Fire(firePos_middle.position, Quaternion.Euler(0, -15, 0) * firePos_middle.forward, speed, damage);

        tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
        tempBullet.Fire(firePos_top.position, Quaternion.Euler(0, -15, 0) * firePos_top.forward, speed, damage);
    }
 
    IEnumerator AttackRnd()
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);

            for (int i = 0; i < rndNum.Count; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    int rnd2 = Random.Range(0, rndNum.Count);
                    int temp = rndNum[i];
                    rndNum[i] = rndNum[rnd2];
                    rndNum[rnd2] = temp;
                }
            }

            switch (rndNum[0])
            {
                case 0:
                    Fire_Bottom_Horizontal();
                    break;
                case 1:
                    Fire_Middle_Horizontal();
                    break;
                case 2:
                    Fire_Top_Horizontal();
                    break;
                case 3:
                    Fire_Middle_Vertical();
                    break;
                case 4:
                    Fire_Left_Vertical();
                    break;
                case 5:
                    Fire_Right_Vertical();
                    break;
            }

            switch (rndNum[1])
            {
                case 0:
                    Fire_Bottom_Horizontal();
                    break;
                case 1:
                    Fire_Middle_Horizontal();
                    break;
                case 2:
                    Fire_Top_Horizontal();
                    break;
                case 3:
                    Fire_Middle_Vertical();
                    break;
                case 4:
                    Fire_Left_Vertical();
                    break;
                case 5:
                    Fire_Right_Vertical();
                    break;
            }
        }
    }

    protected override void Update()
    {
        currentAttackTime -= Time.deltaTime;

        if (currentAttackTime <= 0)
        {
            StopAllCoroutines();
            ResetInfo();
        }
    }
}
