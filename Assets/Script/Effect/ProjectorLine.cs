using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectorLine : MonoBehaviour
{
    private LineRenderer line;
    // Start is called before the first frame update

    private void OnEnable()
    {
        if (line == null)
            line = this.GetComponent<LineRenderer>();

        if (line != null)
        {
            line.SetPosition(0, this.transform.position + Vector3.down * 12);
            line.SetPosition(1, this.transform.position + Vector3.down * 12 + Vector3.up * 30);
        }
        
    }
}
