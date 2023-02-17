using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;

public class PoseEvents : MonoBehaviour
{
    public enum Poses { Aiming, Grab, Unknown}
    public Poses currentPose;

    [SerializeField] protected OVRSkeleton handSkeleton;
    protected List<OVRBone> fingerbones = null;
    [SerializeField] LayerMask interactable;

    private bool hasStarted = false;
    Vector3 indexProximal = Vector3.zero;
    Vector3 indexDistal = Vector3.zero;
    void Start()
    {
        // When the Oculus hand had his time to initialize hand, with a simple coroutine i start a delay of
        // a function to initialize the script
        StartCoroutine(DelayRoutine(Initialize));
    }
    // Coroutine used for delay some function
    public IEnumerator DelayRoutine(Action actionToDo)
    {
        while (!handSkeleton.IsInitialized)
        {
            yield return null;
        }
        actionToDo.Invoke();
    }
    public void Initialize()
    {
        // Check the function for know what it does
        SetSkeleton();
        // After initialize the skeleton set a boolean to true to confirm the initialization
        hasStarted = true;
    }
    public void SetSkeleton()
    {
        // Populate the private list of fingerbones from the current hand we put in the skeleton
        fingerbones = new List<OVRBone>(handSkeleton.Bones);
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasStarted) return;

        switch (currentPose)
        {
            case Poses.Aiming:
                Aim();
                break;
            case Poses.Grab:
                Grab();
                break;
        }
    }

    public void StartAim()
    {
        currentPose = Poses.Aiming;
    }
    void Aim()
    {
        /*Vector3*/ indexProximal = Vector3.zero;
        /*Vector3*/ indexDistal = Vector3.zero;

        foreach(OVRBone bone in fingerbones)
        {
            if (bone.Id == OVRSkeleton.BoneId.Hand_Index1) 
            {
                indexProximal = bone.Transform.localPosition;
            }
            if (bone.Id == OVRSkeleton.BoneId.Hand_Index3) 
            { 
                indexDistal = bone.Transform.localPosition; 
            }
        }

        Ray ray = new Ray(indexProximal, indexDistal);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, interactable))
        {
            Outline outline = hitInfo.transform.GetComponent<Outline>();
            if (outline == null) return;

            outline.enabled = true;
        }
    }
    public void StartGrab()
    {
        currentPose = Poses.Grab;
    }
    void Grab()
    {

    }
    public void EndPose()
    {
        currentPose = Poses.Unknown;
    }

    private void OnDrawGizmos()
    {
        if(currentPose == Poses.Aiming)
        {
            Debug.DrawLine(indexProximal, indexDistal * 100000000, Color.red);
        }
    }
}
