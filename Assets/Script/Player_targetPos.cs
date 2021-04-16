using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_targetPos : MonoBehaviour
{
    private Transform player;
    private Collider playerColl;
    [SerializeField] private float lerpSpeed;

    public void SetLerpSpeed(float speed) { lerpSpeed = speed; }

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.Instance.GetPlayer().transform;
        playerColl = player.GetComponent<Collider>();
        this.transform.position = player.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if(Vector3.Distance(this.transform.position, player.GetComponent<PlayerController>().GetCamPos().position + Vector3.down * 0.01f) > 0.01f)
        //this.transform.position = Vector3.MoveTowards(this.transform.position, player.GetComponent<PlayerController>().GetCamPos().position + Vector3.down * 0.01f, Time.deltaTime * lerpSpeed);
        if(player.GetComponent<PlayerController>().GetIsCrouch())
            this.transform.position = Vector3.MoveTowards(this.transform.position, playerColl.bounds.center + Vector3.up * 0.15f, Time.deltaTime * lerpSpeed);
        else
            this.transform.position = Vector3.MoveTowards(this.transform.position, playerColl.bounds.center + Vector3.up * 0.55f, Time.deltaTime * lerpSpeed);
        //this.transform.position = playerColl.bounds.center + Vector3.up * 0.5f;
    }
}
