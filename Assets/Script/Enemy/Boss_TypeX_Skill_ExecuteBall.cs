using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_TypeX_Skill_ExecuteBall : MonoBehaviour
{
    private Transform target;
    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, target.position, speed * Time.deltaTime);
    }

    public void ActiveTrue(Vector3 startPos, Transform target, float speed)
    {
        this.transform.position = startPos;
        this.target = target;
        this.speed = speed;

        this.gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GameObject temp = GameManager.Instance.GetPoolEffect().GetEffect(EffectType.BulletHit_Trap);
            if (temp != null)
            {
                temp.transform.position = this.transform.position;
                temp.GetComponent<Explosion>().SetDamage(0);
                temp.SetActive(true);
            }

            GameManager.Instance.GetSoundManager().AudioPlayOneShot3D(SoundType.Explosion_2, this.transform.position, false);

            this.gameObject.SetActive(false);
        }
        else if(other.CompareTag("Enemy"))
        {
            other.GetComponent<Boss_TypeX_Skill_Execute>().IncreaseEattenBallNum();
            this.gameObject.SetActive(false);
        }
    }
}
