using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] private Transform[] stageStartPos;

    public Transform GetStageStartPos(int stageNum)
    {
        if (stageNum > stageStartPos.Length) return null;

        return stageStartPos[stageNum - 1];
    }
}
