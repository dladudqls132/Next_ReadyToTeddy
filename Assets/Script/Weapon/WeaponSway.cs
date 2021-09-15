using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    private Vector3 originPos;

    [SerializeField] private Vector3 currentPos;

    [SerializeField] private Vector3 limitPos;


    // Start is called before the first frame update
    void Start()
    {
        originPos = this.transform.localPosition;
        currentPos = originPos;
    }

    // Update is called once per frame
    void Update()
    {
        TrySway();
    }

    private void TrySway()
    {
        if(Input.GetAxisRaw("Mouse X") != 0 || Input.GetAxisRaw("Mouse Y") != 0)
        {
            Swaying();
        }
        else
        {
            BackToOriginPos();
        }
    }

    private void Swaying()
    {
        float moveX =  Input.GetAxisRaw("Mouse X");
        float moveY = Input.GetAxisRaw("Mouse Y");

        moveX = Mathf.Clamp(moveX, -1, 1);
        moveY = Mathf.Clamp(moveY, -1, 1);

        if (!GameManager.Instance.GetPlayer().GetIsSwap())
            currentPos = Vector3.Lerp(currentPos, transform.localPosition + new Vector3(limitPos.x * -moveX, limitPos.y * moveY, 0), Time.deltaTime * 7);
        else
            currentPos = Vector3.Lerp(currentPos, transform.localPosition, Time.deltaTime * 20);
        currentPos.Set(currentPos.x, currentPos.y, transform.localPosition.z);

        transform.localPosition = currentPos;
    }

    private void BackToOriginPos()
    {
        if (!GameManager.Instance.GetPlayer().GetIsSwap())
            currentPos = Vector3.Lerp(currentPos, transform.localPosition, Time.deltaTime * 7);
        else
            currentPos = Vector3.Lerp(currentPos, transform.localPosition, Time.deltaTime * 20);
        currentPos.Set(currentPos.x, currentPos.y, transform.localPosition.z);
        transform.localPosition = currentPos;
    }
}
