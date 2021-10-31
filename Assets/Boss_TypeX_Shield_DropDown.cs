using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_TypeX_Shield_DropDown : Boss_Skill
{
    [SerializeField] private float attackReadyTime;

    [SerializeField] private List<Vector3> attackReadyPos = new List<Vector3>();
    [SerializeField] List<Vector3> tempPos = new List<Vector3>();
    private Vector3 destPos;
    private Quaternion destRot;
    private int attackPosNum;
    private bool isReady;
    private bool isAttack;
    private float speed;
    private bool isReset;

    protected override void Awake()
    {
        base.Awake();

        GameObject[] tempObject = GameObject.FindGameObjectsWithTag("ShieldNode");

        for (int i = 0; i < tempObject.Length; i++)
        {
            attackReadyPos.Add(tempObject[i].transform.position);
            tempPos.Add(attackReadyPos[i]);
        }
    }

    protected override void Start()
    {
        
    }

    private void OnEnable()
    {
        target = this.GetComponent<Boss_TypeX_Shield>().GetTraget();

        for (int i = 0; i < tempPos.Count; i++)
        {
            for (int j = i; j < tempPos.Count; j++)
            { 
                if(Vector3.Distance(target.position, tempPos[i]) > Vector3.Distance(target.position, tempPos[j]))
                {
                    Vector3 temp = tempPos[i];
                    tempPos[i] = tempPos[j];
                    tempPos[j] = temp;
                }    
            }
        }

        attackPosNum = Random.Range(0, tempPos.Count - 1);
        isReset = false;
        isReady = true;
        isAttack = false;
        speed = 3;

        StartCoroutine(AttackReady());
    }

    IEnumerator AttackReady()
    {
        yield return new WaitForSeconds(attackReadyTime / 2);

        isReady = false;

        yield return new WaitForSeconds(attackReadyTime / 2);

        isAttack = true;
    }

    IEnumerator ResetDelay()
    {
        yield return new WaitForSeconds(1);

        ResetInfo();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (isReady)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, tempPos[attackPosNum], Time.deltaTime * 5);
            this.transform.rotation = Quaternion.LookRotation((target.position - this.transform.position).normalized);

            RaycastHit hit;
            if (Physics.Raycast(this.transform.position, this.transform.forward, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Enviroment")))
            {
                destPos = hit.point;
            }
        }

        if(isAttack && !isReset)
        {
            speed += Time.deltaTime * 10;
            this.transform.position = Vector3.MoveTowards(this.transform.position, destPos, speed);

            if(Vector3.Distance(this.transform.position, destPos) < 0.5f)
            {
                isReset = true;
                StartCoroutine(ResetDelay());
            }
        }
    }
}
