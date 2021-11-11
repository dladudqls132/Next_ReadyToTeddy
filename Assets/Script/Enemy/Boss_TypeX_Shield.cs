using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_TypeX_Shield : MonoBehaviour
{
    [SerializeField] private Boss_TypeX owner;
    [SerializeField] private Transform target;
    [SerializeField] private Transform originPos;
    
    [SerializeField] private Renderer[] renderers;
    [SerializeField] private Color emissionColor_normal;
    [SerializeField] private Color emissionColor_angry;
    [SerializeField] private List<Boss_Skill> skills = new List<Boss_Skill>();
    [SerializeField] private Boss_Skill currentSkill;
    [SerializeField] private Queue<Boss_Skill> skillOrder = new Queue<Boss_Skill>();
    [SerializeField] private float coolTime;
    private float currentCoolTime;

    private Animator anim;
    [SerializeField] private bool isOn;
    [SerializeField] private int currentPhase;

    public Transform GetTarget() { return target; }

    public int GetCurrentPhase()
    {
        return currentPhase;
    }

    public void SetPickUp()
    {
        anim.SetBool("isOn", true);
    }

    void SetOn()
    {
        isOn = true;
    }

    private void Start()
    {
        anim = this.GetComponent<Animator>();

        skills.AddRange(this.GetComponents<Boss_Skill>());
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null)
            target = owner.GetTarget().parent;

        if (!isOn)
        {
            return;
        }

        currentPhase = owner.GetCurrentPhase();

        for (int i = 0; i < skills.Count; i++)
        {
            if (skills[i] != currentSkill)
            {
                if (skills[i].CoolDown())
                {
                    if (!skillOrder.Contains(skills[i]))
                    {
                        skillOrder.Enqueue(skills[i]);
                    }
                }
            }
        }


        if (currentPhase != 5)
        {
            if (currentSkill != null)
            {
                if (!currentSkill.enabled)
                {
                    currentSkill = null;
                }
            }
            else
            {
                currentCoolTime -= Time.deltaTime;

                if (currentCoolTime <= 0 && skillOrder.Count != 0)
                {
                    currentSkill = skillOrder.Dequeue();
                    currentSkill.Use();
                    currentCoolTime = coolTime;
                }
            }
        }

        if (!currentSkill)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, originPos.position, Time.deltaTime * 2.5f);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, originPos.rotation, Time.deltaTime * 5);
        }
    }

    private void LateUpdate()
    {

    }
}
