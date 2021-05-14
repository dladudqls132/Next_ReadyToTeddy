using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor.Animations;


public class Editor_AnimationSave : MonoBehaviour
{
    private HandAnimController handAnimController;
    private Transform weaponLeftGrip;
    private Transform weaponRightGrip;
    private PlayerController player;

    private void Start()
    {
        player = GameManager.Instance.GetPlayer();
        handAnimController = FindObjectOfType<HandAnimController>();
        weaponLeftGrip = handAnimController.weaponLeftGrip;
        weaponRightGrip = handAnimController.weaponRightGrip;
    }

    //[ContextMenu("Save weapon pose")]
    //void SaveWeaponPose()
    //{
    //    GameObjectRecorder recorder = new GameObjectRecorder(gameObject);

    //    //recorder.BindComponentsOfType<Transform>(this.gameObject, false);
    //    recorder.BindComponentsOfType<Transform>(weaponLeftGrip.gameObject, false);
    //    recorder.BindComponentsOfType<Transform>(weaponRightGrip.gameObject, false);

    //    recorder.TakeSnapshot(0.0f);
    //    recorder.SaveToClip(player.GetWeaponGameObject().GetComponent<Gun>().weaponAnimation);
    //    //recorder.SaveToClip(temp);
    //}
}
