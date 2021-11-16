using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_TypeX_Shield_Laser : Boss_Skill
{
    [SerializeField] GameObject laser;
    [SerializeField] private float attackReadyTime;
    [SerializeField] private bool isReady;
    [SerializeField] private float attackTick;
    private bool isAttack;

    [SerializeField] private Transform attackPos;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        
    }

    private void OnEnable()
    {
        target = this.GetComponent<Boss_TypeX_Shield>().GetTarget();
        tempRot = Quaternion.LookRotation(this.transform.up);
        isReady = true;
        isAttack = false;
      

        StartCoroutine(AttackReady());
    }

    IEnumerator AttackReady()
    {
        yield return new WaitForSeconds(attackReadyTime / 2);

        isReady = false;
        laser.SetActive(true);

        yield return new WaitForSeconds(attackReadyTime);

        laser.GetComponent<Boss_TypeX_Skill_Laser>().SetAttackTrue(damage, attackTick);
        StartCoroutine(ResetDelay());
        isAttack = true;
    }

    IEnumerator ResetDelay()
    {
        yield return new WaitForSeconds(5.5f);

        ResetInfo();
    }

    public override void ResetInfo()
    {
        laser.GetComponent<Boss_TypeX_Skill_Laser>().SetAttackFalse();

        laser.SetActive(false);

        StopAllCoroutines();
        base.ResetInfo();
    }

    Quaternion tempRot;
    protected override void Update()
    {
        if (isReady)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, attackPos.position, Time.deltaTime * 10);
            tempRot = Quaternion.Lerp(tempRot, Quaternion.LookRotation((target.position - this.transform.position).normalized), Time.deltaTime * 10);
            this.transform.rotation = tempRot;
            this.transform.localRotation = Quaternion.Euler(this.transform.localEulerAngles.x + 89, this.transform.localEulerAngles.y, this.transform.localEulerAngles.z);
        }
        else
        {
            tempRot = Quaternion.RotateTowards(tempRot, Quaternion.LookRotation((target.position - this.transform.position).normalized), Time.deltaTime * 15);
            this.transform.rotation = tempRot;
            this.transform.localRotation = Quaternion.Euler(this.transform.localEulerAngles.x + 89, this.transform.localEulerAngles.y, this.transform.localEulerAngles.z);
        }


    }
    // Update is called once per frame
  
}
