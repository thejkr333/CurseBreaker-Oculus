using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIInteraction : MonoBehaviour
{


    // Sourced from https://forum.unity.com/threads/how-to-oculus-quest-hand-tracking-pointerpose-pinch.1170538/
    public OVRInputModule _OVRInputModule;

    public OVRRaycaster _OVRRaycaster;

    public OVRHand _OVRHand;


    void Start()
    {

        _OVRInputModule.rayTransform = _OVRHand.PointerPose;
        _OVRRaycaster.pointer = _OVRHand.PointerPose.gameObject;

    }

    void Update()
    {
        _OVRInputModule.rayTransform = _OVRHand.PointerPose;
        _OVRRaycaster.pointer = _OVRHand.PointerPose.gameObject;
    }
}
