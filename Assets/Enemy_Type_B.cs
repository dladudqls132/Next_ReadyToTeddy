using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Type_B : Enemy
{
    [SerializeField] private Transform[] firePos;
    [SerializeField] private float fireRate;
    private float currentFireRate;
    [SerializeField] private Transform projector;
    [SerializeField] private float bombTime;
    private float currentBombTime;
    private bool isAttack;
    [SerializeField] private float bombSize;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        projector = Instantiate(projector);

        currentFireRate = Random.Range(0, fireRate);
    }

    // Update is called once per frame
    void Update()
    {
        CheckingPlayer();

        if (isRigidity)
        {
            currentRigidityTime += Time.deltaTime;
            if (currentRigidityTime >= rigidityTime)
            {
                isRigidity = false;
                currentRigidityTime = 0;
            }
        }

        if (isAttack)
        {
            currentBombTime += Time.deltaTime;

            projector.GetComponent<Projector>().orthographicSize = Mathf.Lerp(projector.GetComponent<Projector>().orthographicSize, bombSize, Time.deltaTime * 10);
            projector.GetChild(0).GetComponent<Projector>().orthographicSize = projector.GetComponent<Projector>().orthographicSize;

            if (currentBombTime >= bombTime)
            {
                currentBombTime = 0;
                projector.GetComponent<Projector>().orthographicSize = 1;
                projector.GetChild(0).GetComponent<Projector>().orthographicSize = projector.GetComponent<Projector>().orthographicSize;
                GameObject temp = GameManager.Instance.GetPoolEffect().GetEffect(EffectType.Explosion_bomb_large);
                RaycastHit hit;
                if (Physics.Raycast(projector.position, Vector3.down, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Enviroment")))
                {
                    temp.transform.position = hit.point;

                    Collider[] c = Physics.OverlapSphere(hit.point, bombSize / 2, 1 << LayerMask.NameToLayer("Player"));
                    if (c.Length != 0)
                    {
                        c[0].GetComponent<PlayerController>().DecreaseHp(damage);
                    }
                    temp.SetActive(true);

                }
                projector.gameObject.SetActive(false);
                isAttack = false;
            }
        }

        if (!isDetect || isDead || isRigidity)
        {
            return;
        }

        currentFireRate += Time.deltaTime;

        if (currentFireRate >= fireRate)
        {
            currentFireRate = 0;

            Attack();
        }

        //if(isAttack)
        //{
        //    currentBombTime += Time.deltaTime;

        //    projector.GetComponent<Projector>().orthographicSize = Mathf.Lerp(projector.GetComponent<Projector>().orthographicSize, bombSize, Time.deltaTime * 10);
        //    projector.GetChild(0).GetComponent<Projector>().orthographicSize = projector.GetComponent<Projector>().orthographicSize;

        //    if (currentBombTime >= bombTime)
        //    {
        //        currentBombTime = 0;
        //        projector.GetComponent<Projector>().orthographicSize = 1;
        //        projector.GetChild(0).GetComponent<Projector>().orthographicSize = projector.GetComponent<Projector>().orthographicSize;
        //        GameObject temp = GameManager.Instance.GetPoolEffect().GetEffect(EffectType.Explosion_bomb_large);
        //        RaycastHit hit;
        //        if(Physics.Raycast(projector.position, Vector3.down, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Enviroment")))
        //        {
        //            temp.transform.position = hit.point;

        //            Collider[] c = Physics.OverlapSphere(hit.point, bombSize / 2, 1 << LayerMask.NameToLayer("Player"));
        //            if (c.Length != 0)
        //            {
        //                c[0].GetComponent<PlayerController>().DecreaseHp(damage);
        //            }
        //            temp.SetActive(true);

        //        }
        //        projector.gameObject.SetActive(false);
        //        isAttack = false;
        //    }
        //}

        if (currentFireRate < fireRate / 2)
        {
            foreach (Renderer r in renderers)
            {
                r.material.SetColor("_EmissionColor", Color.Lerp(r.material.GetColor("_EmissionColor"), (emissionColor_normal * 35f), Time.deltaTime * 6));
            }
        }
        else
        {
            foreach (Renderer r in renderers)
            {
                r.material.SetColor("_EmissionColor", Color.Lerp(r.material.GetColor("_EmissionColor"), (emissionColor_angry * 35f) * (currentFireRate / fireRate), Time.deltaTime * 4));
            }
        }

        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(new Vector3(target.position.x, this.transform.position.y, target.position.z) - this.transform.position), Time.deltaTime * 10);
    }

    public override void SetDead(bool value)
    {
        isDead = value;

        if (isDead)
        {
            GameObject temp = GameManager.Instance.GetPoolEffect().GetEffect(EffectType.Explosion_destroy);
            temp.transform.position = this.transform.position;
            temp.SetActive(true);
            projector.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
        }
    }

    void Attack()
    {
        isAttack = true;

        projector.position = target.position + new Vector3(target.parent.GetComponent<Rigidbody>().velocity.x, 0, target.parent.GetComponent<Rigidbody>().velocity.z) / 2 + Vector3.up * 3;

        projector.gameObject.SetActive(true);
    }
}
