using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_TypeX_Skill_RandomShot : Boss_Skill
{
    [SerializeField] private Transform firePos_bottom;
    [SerializeField] private Transform firePos_middle;
    [SerializeField] private Transform firePos_top;
    [SerializeField] private Transform firePos_top2;
    [SerializeField] private GameObject wall_1;
    [SerializeField] private GameObject wall_2;
    [SerializeField] private GameObject barrier;
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
        wall_1.SetActive(true);
        wall_2.SetActive(true);
        barrier.SetActive(true);

        StartCoroutine(AttackRnd());
        anim.SetBool("isRandomShot", true);
    }

    public override void ResetInfo()
    {
        wall_1.SetActive(false);
        wall_2.SetActive(false);
        barrier.SetActive(false);

        anim.SetBool("isRandomShot", false);

        base.ResetInfo();
    }

    void Fire_Bottom_Horizontal()
    {
        Bullet_Boss tempBullet = null;

        for (int i = 0; i < 10; i++)
        {
            tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
            tempBullet.Fire(firePos_bottom.position, Quaternion.Euler(0, 3 * i, 0) * firePos_bottom.forward, speed, damage);

            if (i != 0)
            {
                tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
                tempBullet.Fire(firePos_bottom.position, Quaternion.Euler(0, 3 * -i, 0) * firePos_bottom.forward, speed, damage);
            }
        }
        
    }
    
    void Fire_Middle_Horizontal()
    {
        Bullet_Boss tempBullet = null;

        for (int i = 0; i < 10; i++)
        {
            tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
            tempBullet.Fire(firePos_middle.position, Quaternion.Euler(0, 3f * i, 0) * firePos_middle.forward, speed, damage);

            if (i != 0)
            {
                tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
                tempBullet.Fire(firePos_middle.position, Quaternion.Euler(0,3f * -i, 0) * firePos_middle.forward, speed, damage);
            }
        }
    }

    void Fire_Top_Horizontal()
    {
        Bullet_Boss tempBullet = null;

        for (int i = 0; i < 10; i++)
        {
            tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
            tempBullet.Fire(firePos_top.position, Quaternion.Euler(0, 3f * i, 0) * firePos_top.forward, speed, damage);

            if (i != 0)
            {
                tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
                tempBullet.Fire(firePos_top.position, Quaternion.Euler(0, 3f * -i, 0) * firePos_top.forward, speed, damage);
            }

            tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
            tempBullet.Fire(firePos_top2.position, Quaternion.Euler(0, 3f * i, 0) * firePos_top2.forward, speed, damage);

            if (i != 0)
            {
                tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
                tempBullet.Fire(firePos_top2.position, Quaternion.Euler(0, 3f * -i, 0) * firePos_top2.forward, speed, damage);
            }
        }
    }

    void Fire_Middle_Vertical()
    {
        Bullet_Boss tempBullet = null;

        for (int i = 0; i < 5; i++)
        {
            tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
            tempBullet.Fire(firePos_top2.position, Quaternion.Euler(0, 3f * i, 0) * firePos_top2.forward, speed, damage);

            tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
            tempBullet.Fire(firePos_top.position, Quaternion.Euler(0, 3f * i, 0) * firePos_top.forward, speed, damage);

            tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
            tempBullet.Fire(firePos_middle.position, Quaternion.Euler(0, 3f * i, 0) * firePos_middle.forward, speed, damage);

            tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
            tempBullet.Fire(firePos_bottom.position, Quaternion.Euler(0, 3f * i, 0) * firePos_bottom.forward, speed, damage);

            if (i != 0)
            {
                tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
                tempBullet.Fire(firePos_top2.position, Quaternion.Euler(0, 3f * -i, 0) * firePos_top2.forward, speed, damage);

                tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
                tempBullet.Fire(firePos_top.position, Quaternion.Euler(0, 3f * -i, 0) * firePos_top.forward, speed, damage);

                tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
                tempBullet.Fire(firePos_middle.position, Quaternion.Euler(0, 3f * -i, 0) * firePos_middle.forward, speed, damage);

                tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
                tempBullet.Fire(firePos_bottom.position, Quaternion.Euler(0, 3f * -i, 0) * firePos_bottom.forward, speed, damage);
            }
        }
    }

    void Fire_Left_Vertical()
    {
        Bullet_Boss tempBullet = null;

        for (int i = 2; i < 10; i++)
        {
            tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
            tempBullet.Fire(firePos_top2.position, Quaternion.Euler(0, 3f * i, 0) * firePos_top2.forward, speed, damage);

            tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
            tempBullet.Fire(firePos_top.position, Quaternion.Euler(0, 3f * i, 0) * firePos_top.forward, speed, damage);

            tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
            tempBullet.Fire(firePos_middle.position, Quaternion.Euler(0, 3f * i, 0) * firePos_middle.forward, speed, damage);

            tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
            tempBullet.Fire(firePos_bottom.position, Quaternion.Euler(0, 3f * i, 0) * firePos_bottom.forward, speed, damage);
        }
    }

    void Fire_Right_Vertical()
    {
        Bullet_Boss tempBullet = null;

        for (int i = 2; i < 10; i++)
        {
            tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
            tempBullet.Fire(firePos_top2.position, Quaternion.Euler(0, 3f * -i, 0) * firePos_top2.forward, speed, damage);

            tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
            tempBullet.Fire(firePos_top.position, Quaternion.Euler(0, 3f * -i, 0) * firePos_top.forward, speed, damage);

            tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
            tempBullet.Fire(firePos_middle.position, Quaternion.Euler(0, 3f * -i, 0) * firePos_middle.forward, speed, damage);

            tempBullet = GameManager.Instance.GetPoolBullet().GetBullet(BulletType.Energy).GetComponent<Bullet_Boss>();
            tempBullet.Fire(firePos_bottom.position, Quaternion.Euler(0, 3f * -i, 0) * firePos_bottom.forward, speed, damage);
        }
    }
 
    IEnumerator AttackRnd()
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);

            int rnd1 = Random.Range(0, 3);
            int rnd2 = Random.Range(0, 3);

            switch(rnd1)
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
            }

            switch(rnd2)
            {
                case 0:
                    Fire_Left_Vertical();
                    break;
                case 1:
                    Fire_Middle_Vertical();
                    break;
                case 2:
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
