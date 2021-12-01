using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_TypeX_Skill_Bombing : Boss_Skill
{
    [SerializeField] private int bombNum_1;
    [SerializeField] private int bombNum_2;
    [SerializeField] private float bombTime;
    [SerializeField] private float attackRange;
    private int fireNum;

    protected override void Start()
    {
        base.Start();
    }

    public override void Use()
    {
        base.Use();

        if (this.GetComponent<Boss_TypeX>().GetCurrentPhase() >= 3)
        {
            int rndNum = Random.Range(0, 2);

            if (rndNum == 0)
                anim.SetTrigger("Bombing");
            else
                anim.SetTrigger("Bombing2");
        }
        else
        {
            anim.SetTrigger("Bombing2");
        }

    }

    public override void ResetInfo()
    {
        fireNum = 0;
        base.ResetInfo();
    }

    IEnumerator Fire_Bomb1_Delay(float time)
    {
        yield return new WaitForSeconds(time);

        GameObject tempProjector = GameManager.Instance.GetPoolEffect().GetEffect(EffectType.Projector_Explosion_Large);
        Vector2 rndVec = Random.insideUnitCircle * attackRange;
        tempProjector.GetComponent<Explosion_Large>().SetActive(this.transform.position + new Vector3(rndVec.x, 0, rndVec.y) + Vector3.up * 3, bombTime, damage);
    }

    void Fire_Bomb1()
    {
        GameManager.Instance.GetSoundManager().AudioPlayOneShot3D(SoundType.Explosion_Fire, this.transform.position + Vector3.up * 3, false);

        for (int i = 0; i < bombNum_1; i++)
        {
            StartCoroutine(Fire_Bomb1_Delay(i * 0.31f));
        }
    }

    void Fire_Bomb2()
    {
        for (int i = 0; i < bombNum_2; i++)
        {
            for (int j = 1; j < 4; j++)
            {
                GameObject tempProjector = GameManager.Instance.GetPoolEffect().GetEffect(EffectType.Projector_Explosion_Large);
                tempProjector.GetComponent<Explosion_Large>().SetActive(this.transform.position + Quaternion.Euler(0, (360 / bombNum_2 / 3) * fireNum + 360 / bombNum_2 * i, 0) * Vector3.forward * j * 10 + Vector3.up * 3, bombTime, damage);
            }
        }

        GameManager.Instance.GetSoundManager().AudioPlayOneShot3D(SoundType.Explosion_Fire, this.transform.position + Vector3.up * 3, false);
        fireNum++;
    }

    protected override void Update()
    {
       
        

        //currentAttackTime -= Time.deltaTime;

        //anim.SetBool("isAttack_EnergyBall_LeftHand", true);

        //if (currentAttackTime <= 0)
        //{
        //    ResetInfo();
        //}
    }
}
