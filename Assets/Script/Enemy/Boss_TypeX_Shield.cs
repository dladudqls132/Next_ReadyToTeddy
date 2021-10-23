using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_TypeX_Shield : MonoBehaviour
{
    [SerializeField] private Transform originPos;
    [SerializeField] private Transform mesh;
    [SerializeField] private Renderer[] renderers;
    [SerializeField] private Color emissionColor_normal;
    [SerializeField] private Color emissionColor_angry;

    private Vector3 tempPos;
    private Vector3 originPos_mesh;
    private bool isOn;
    private bool isAttack;
    private float speed = 2;

    void SetOn()
    {
        tempPos = mesh.localPosition;
        isOn = true;
    }

    private void Start()
    {
        originPos_mesh = mesh.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            this.GetComponent<Animator>().SetBool("isOn", true);
        }

        if (isOn)
        {
            foreach (Renderer r in renderers)
            {
                r.material.SetColor("_EmissionColor", Color.Lerp(r.material.GetColor("_EmissionColor"), (emissionColor_angry * 35f), Time.deltaTime * 2));
            }

            speed += Time.deltaTime * 3;
            speed = Mathf.Clamp(speed, 0, 15);
            this.transform.position = Vector3.Lerp(this.transform.position, originPos.position, Time.deltaTime * speed);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, originPos.rotation, Time.deltaTime * speed);

        }

    }

    private void LateUpdate()
    {
        if(isOn)
        {
            tempPos = Vector3.Lerp(tempPos, originPos_mesh, Time.deltaTime * speed);
            mesh.localPosition = tempPos;
        }
    }
}
