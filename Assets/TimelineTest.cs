using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineTest : MonoBehaviour
{
    [SerializeField] private PlayableDirector pd;
    [SerializeField] private Camera mainCam;
    [SerializeField] private Camera timelineCam;

    // Start is called before the first frame update
    void Start()
    {
        pd = this.GetComponent<PlayableDirector>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            //mainCam.enabled = false;
            //timelineCam.enabled = true;
            pd.Play();
        }
    }
}
