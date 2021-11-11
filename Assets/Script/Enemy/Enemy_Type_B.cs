using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Type_B : Enemy
{
    [SerializeField] private Transform[] firePos;
    [SerializeField] private float fireRate;
    private float currentFireRate;
    [SerializeField] private float bombTime;

    [SerializeField] private float bombSize;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

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

        if (currentFireRate < fireRate / 1.5f)
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
                r.material.SetColor("_EmissionColor", Color.Lerp(r.material.GetColor("_EmissionColor"), (emissionColor_angry * 35f) * (currentFireRate / fireRate), Time.deltaTime * 3));
            }
        }

        anim.SetFloat("FireRate", currentFireRate / fireRate);

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

            GameManager.Instance.GetSoundManager().AudioPlayOneShot3D(SoundType.Explosion_1, this.transform.position, false);

            this.gameObject.SetActive(false);
        }
    }

    //void Destroy()
    //{
    //    this.gameObject.SetActive(false);
    //}

    void Attack()
    {
        GameManager.Instance.GetPoolEffect().GetEffect(EffectType.Projector_Explosion_Large).GetComponent<Explosion_Large>().SetActive(target.position + new Vector3(target.parent.GetComponent<Rigidbody>().velocity.x, 0, target.parent.GetComponent<Rigidbody>().velocity.z) / 2 + Vector3.up * 3, bombTime, damage);

        anim.SetTrigger("Attack");
    }
}
