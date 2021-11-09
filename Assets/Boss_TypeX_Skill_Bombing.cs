using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_TypeX_Skill_Bombing : Boss_Skill
{
    [SerializeField] private int bombNum_1;
    [SerializeField] private int bombNum_2;
    [SerializeField] private float bombTime;
    [SerializeField] private float attackRange;

    protected override void Start()
    {
        base.Start();
    }

    public override void Use()
    {
        base.Use();

        int rndNum = Random.Range(0, 2);

        if (rndNum == 0)
            anim.SetTrigger("Bombing");
        else
            anim.SetTrigger("Bombing2");

    }

    protected override void ResetInfo()
    {
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
        for (int i = 0; i < bombNum_1; i++)
        {
            StartCoroutine(Fire_Bomb1_Delay(i * 0.2f));
        }
    }

    void Fire_Bomb2()
    {
        for (int i = 0; i < bombNum_2; i++)
        {
            Vector3 rndDir = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f)).normalized;
            if (rndDir == Vector3.zero)
                rndDir = new Vector3(1, 0, 1);

            for (int j = 0; j < 4; j++)
            {
                GameObject tempProjector = GameManager.Instance.GetPoolEffect().GetEffect(EffectType.Projector_Explosion_Large);
                tempProjector.GetComponent<Explosion_Large>().SetActive(this.transform.position + rndDir * j * 10 + Vector3.up * 3, bombTime, damage);
            }
        }
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
