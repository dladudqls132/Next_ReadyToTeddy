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
    [SerializeField] private Boss_Skill[] skills;
    [SerializeField] private Boss_Skill currentSkill;

    private Animator anim;
    [SerializeField] private bool isOn;
    [SerializeField] private int currentPhase;

    public Transform GetTraget() { return target; }

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

        target = owner.GetTarget();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOn)
        {
            return;
        }

        currentPhase = owner.GetCurrentPhase();

        if (currentSkill != null)
        {
            if (!currentSkill.enabled)
            {
                currentSkill = null;
            }
        }

        if (currentSkill) return;

        for (int i = 0; i < skills.Length; i++)
        {
            if (skills[i].CoolDown())
            {
                if (currentSkill == null)
                {
                    currentSkill = skills[i];
                    currentSkill.Use();
                    break;
                }
            }
        }

        this.transform.position = Vector3.Lerp(this.transform.position, originPos.position, Time.deltaTime * 5);
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, originPos.rotation, Time.deltaTime * 5);
    }

    private void LateUpdate()
    {

    }
}
