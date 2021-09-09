using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chain : MonoBehaviour
{
    private LineRenderer line;
    private Transform start;
    private Transform end;
    private float linkTime;
    private float currentLinkTime;
    private bool link;

    // Start is called before the first frame update
    void Start()
    {
        line = this.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(link)
        {
            line.SetPosition(0, start.position);
            line.SetPosition(1, end.position);

            currentLinkTime += Time.deltaTime;

            if(currentLinkTime >= linkTime)
            {
                currentLinkTime = 0;
                link = false;
                Destroy(this.gameObject);
            }
        }
    }

    public void SetLine(Transform start, Transform end, float time)
    {
        link = true;
        this.start = start;
        this.end = end;
        this.linkTime = time;
    }
}
