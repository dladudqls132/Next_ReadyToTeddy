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
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, playerColl.bounds.center + Vector3.up * 0.5f, Time.deltaTime * lerpSpeed);
        //this.transform.position = playerColl.bounds.center + Vector3.up * 0.5f;
    }
}
