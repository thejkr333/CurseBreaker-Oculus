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
    LineRenderer lineRenderer;
    protected List<OVRBone> fingerbones = null;
    [SerializeField] LayerMask interactable;

    private bool hasStarted = false;
    bool attracting = false;
    Vector3 indexProximal = Vector3.zero;
    Vector3 indexTip = Vector3.zero;

    Outline lastOutline;
    void Start()
    {
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
        }
    }

    public void StartAim()
    {
        currentPose = Poses.Aiming;
    }
    void Aim()
    {
        lineRenderer.enabled = true;
        /*Vector3*/
        indexProximal = Vector3.zero;
        /*Vector3*/ indexTip = Vector3.zero;

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

        lineRenderer.SetPosition(0, indexTip/* + indexProximal*/);
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
        lineRenderer.enabled = false;

        Invoke(nameof(EndAim), 2);
        currentPose = Poses.Grab;
    }
    void Grab()
    {
        if(!attracting) if (lastOutline == null || lastOutline.enabled == false) return;

        Rigidbody rb = lastOutline.GetComponent<Rigidbody>();
        if (rb == null) return;

        if (!attracting) StartCoroutine(AttractObject(rb));
    }

    IEnumerator AttractObject(Rigidbody objectRb)
    {
        attracting = true;

        while(Vector3.Distance(objectRb.transform.position, handSkeleton.transform.position) > .3f)
        {
            objectRb.AddForce((handSkeleton.transform.position - objectRb.transform.position).normalized * 3);

            yield return 0;
        }

        objectRb.velocity = Vector3.zero;
        objectRb.transform.SetParent(handSkeleton.transform);

        attracting = false;
    }
    public void EndPose()
    {
        currentPose = Poses.Unknown;
        lineRenderer.enabled = false;

        Invoke(nameof(EndAim), 2);
    }

}
