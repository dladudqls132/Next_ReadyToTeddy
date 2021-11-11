using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion_Large : MonoBehaviour
{
    private Projector projector;
    private float bombTime;
    private float currentBombTime;
    private float bombSize;
    private FPPCamController mainCam;
    private float damage;
    private bool isAttack;

    // Start is called before the first frame update
    void Start()
    {
        projector = this.GetComponent<Projector>();
        mainCam = Camera.main.transform.GetComponent<FPPCamController>();
    }

    public void SetActive(Vector3 pos, float bombTime, float damage)
    {
        isAttack = true;

        this.transform.position = pos;

        this.bombTime = bombTime;
        this.bombSize = 10;
        this.damage = damage;

        projector.GetComponent<Projector>().orthographicSize = 1;
        this.transform.GetChild(0).GetComponent<Projector>().orthographicSize = projector.orthographicSize;
        currentBombTime = 0;

        this.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttack)
        {
            if (currentBombTime >= bombTime)
            {
                GameObject temp = GameManager.Instance.GetPoolEffect().GetEffect(EffectType.Explosion_bomb_large);
                RaycastHit hit;

                if (Physics.Raycast(this.transform.position, Vector3.down, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Enviroment")))
                {
                    temp.transform.position = hit.point;
                    mainCam.Shake(0.2f, 0.25f, false);

                    temp.GetComponent<Explosion>().SetDamage(damage);
                    temp.SetActive(true);
                    GameManager.Instance.GetSoundManager().AudioPlayOneShot3D(SoundType.Explosion, hit.point, false);
                }

                isAttack = false;
                projector.gameObject.SetActive(false);
            }
            else
            {
                currentBombTime += Time.deltaTime;

                projector.orthographicSize = Mathf.Lerp(projector.orthographicSize, bombSize, Time.deltaTime * 10);
                this.transform.GetChild(0).GetComponent<Projector>().orthographicSize = projector.orthographicSize;
            }
        }
    }
}
