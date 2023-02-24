using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;

public class PoseEvents : MonoBehaviour
{
    public enum Poses { Aiming, Grab, OpenHand, Unknown}
    public Poses currentPose;

    [SerializeField] protected OVRSkeleton handSkeleton;
    PoseGrab poseGrab;
    LineRenderer lineRenderer;
    protected List<OVRBone> fingerbones = null;
    [SerializeField] LayerMask interactable, grabbed;

    private bool hasStarted = false;
    [SerializeField] bool attracting = false;
    Vector3 indexProximal = Vector3.zero;
    Vector3 indexTip = Vector3.zero;

    [SerializeField] Outline lastOutline;
    void Start()
    {
        poseGrab = handSkeleton.GetComponent<PoseGrab>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
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

            case Poses.OpenHand:
                OpenHand();
                break;

            case Poses.Unknown:
                break;
        }
    }

    public void StartAim()
    {
        EndGrab();

        currentPose = Poses.Aiming;
    }
    void Aim()
    {
        lineRenderer.enabled = true;
        indexProximal = Vector3.zero;
        indexTip = Vector3.zero;

        foreach(OVRBone bone in fingerbones)
        {
            if (bone.Id == OVRSkeleton.BoneId.Hand_Index1) 
            {
                indexProximal = bone.Transform.position;
                continue;
            }
            if (bone.Id == OVRSkeleton.BoneId.Hand_IndexTip) 
            {
                indexTip = bone.Transform.position;
                continue;
            }
        }

        lineRenderer.SetPosition(0, indexTip);
        lineRenderer.SetPosition(1, (indexTip - indexProximal) * 100000000);

        Ray ray = new Ray(indexTip, indexTip - indexProximal);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, interactable))
        {
            Outline outline = hitInfo.transform.GetComponent<Outline>();
            if (outline == null) return;

            outline.enabled = true;
            if (lastOutline != null && lastOutline != outline && lastOutline.enabled) { lastOutline.enabled = false; lastOutline = null; }

            lastOutline = outline;
        }
    }
    void EndAim()
    {
        lineRenderer.enabled = false;

        if (lastOutline != null && lastOutline.enabled) lastOutline.enabled = false;
        lastOutline = null;
    }
    public void StartGrab()
    {
        currentPose = Poses.Grab;
    }
    void Grab()
    {
        if (attracting) return;

        if (lastOutline != null && lastOutline.enabled == true)
        {
            Rigidbody rb = lastOutline.GetComponent<Rigidbody>();
            if (rb == null) return;

            StartCoroutine(AttractObject(rb));
        }
        else
        {
            EndAim();
            poseGrab.DetectGrabbing(true);
            return;
        }  
    }
    void EndGrab()
    {
        poseGrab.IsReleasing();
        poseGrab.DetectGrabbing(false);
    }
    public void StartOpenHand()
    {
        EndGrab();
        EndAim();

        currentPose = Poses.OpenHand;
    }
    void OpenHand()
    {

    }

    IEnumerator AttractObject(Rigidbody objectRb)
    {
        EndAim();
        poseGrab.DetectGrabbing(false);

        objectRb.useGravity = false;

        attracting = true;
        GameObject obj = objectRb.gameObject;

        LayerMask objectLayer = obj.layer;
        obj.layer = grabbed;

        while(Vector3.Distance(obj.transform.position, handSkeleton.transform.position) > .3f)
        {
            objectRb.AddForce((handSkeleton.transform.position - obj.transform.position).normalized * 3);

            yield return 0;
        }

        objectRb.velocity = Vector3.zero;
        obj.layer = objectLayer;
        objectRb.useGravity = true;
        //objectRb.transform.SetParent(handSkeleton.transform);

        poseGrab.DetectGrabbing(true);

        attracting = false;
    }
    public void EndPoses()
    {
        currentPose = Poses.Unknown;
        lineRenderer.enabled = false;

        EndGrab();
        Invoke(nameof(EndAim), 2);
    }

}
