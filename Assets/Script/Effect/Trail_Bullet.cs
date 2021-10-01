using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail_Bullet : MonoBehaviour
{
    Vector3 dir;

    private void Update()
    {
        this.transform.position = this.transform.position + dir * 450 * Time.deltaTime;
    }
    public void SetFire(Vector3 startPos, Vector3 dir)
    {
        this.transform.position = startPos;
        this.dir = dir;

        this.GetComponent<TrailRenderer>().Clear();
        this.gameObject.SetActive(true);

        StartCoroutine(ActiveFalse());
    }

    IEnumerator ActiveFalse()
    {
        yield return new WaitForSeconds(1f);
        
        this.gameObject.SetActive(false);
    }
}
