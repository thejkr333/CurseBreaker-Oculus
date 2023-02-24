using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;

public class PoseEvents : MonoBehaviour
{
    public enum Poses { Aiming, Grab, OpenHand, SpellSelect, Unknown}
    public Poses currentPose;

    [SerializeField] protected OVRSkeleton handSkeleton;
    PoseGrab poseGrab;
    LineRenderer lineRenderer;
    TrailRenderer trailRenderer;
    protected List<OVRBone> fingerbones = null;
    [SerializeField] LayerMask interactable, grabbed;

    private bool hasStarted = false;
    [SerializeField] bool attracting = false;
    Vector3 indexProximal = Vector3.zero;
    Vector3 indexTip = Vector3.zero;

    [SerializeField] Outline lastOutline;
    Rigidbody attractedObjRb;
    LayerMask objectLayer;
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
        foreach (var bone in fingerbones)
        {
            if(bone.Id == OVRSkeleton.BoneId.Hand_MiddleTip)
            {
                trailRenderer = bone.Transform.gameObject.AddComponent<TrailRenderer>();
                trailRenderer.enabled = false;
                trailRenderer.time = 2;
                trailRenderer.minVertexDistance = .0001f;
                trailRenderer.startWidth = .01f;
            }
        }
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

            case Poses.SpellSelect:
                SpellSelect();
                break;

            case Poses.Unknown:
                break;
        }
    }

    #region Aim
    public void StartAim()
    {
        EndGrab();
        EndSpellSelect();

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
    #endregion Aim

    #region Grab
    public void StartGrab()
    {
        EndSpellSelect();

        currentPose = Poses.Grab;
    }
    void Grab()
    {
        if (attracting) return;

        if (lastOutline != null && lastOutline.enabled == true)
        {
            Rigidbody rb = lastOutline.GetComponent<Rigidbody>();
            if (rb == null) return;

            attractedObjRb = rb;
            StartCoroutine(AttractObject());
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
        if (attracting)
        {
            StopCoroutine(AttractObject());
            attracting = false;

            attractedObjRb.useGravity = true;
            attractedObjRb.gameObject.layer = objectLayer;
            objectLayer = 0;
            attractedObjRb = null;
        }

        poseGrab.IsReleasing();
        poseGrab.DetectGrabbing(false);
    }
    IEnumerator AttractObject()
    {
        EndAim();
        poseGrab.DetectGrabbing(false);

        attractedObjRb.useGravity = false;

        attracting = true;
        GameObject obj = attractedObjRb.gameObject;

        objectLayer = obj.layer;
        int LayerGrabbed = LayerMask.NameToLayer("Grabbed");
        obj.layer = LayerGrabbed;
        Debug.Log("Current layer: " + obj.layer);

        while (Vector3.Distance(obj.transform.position, handSkeleton.transform.position) > .3f)
        {
            if(attractedObjRb != null) attractedObjRb.AddForce((handSkeleton.transform.position - obj.transform.position).normalized * 3);

            yield return 0;
        }

        attractedObjRb.velocity = Vector3.zero;
        obj.layer = objectLayer;
        objectLayer = 0;
        attractedObjRb.useGravity = true;
        //objectRb.transform.SetParent(handSkeleton.transform);

        poseGrab.DetectGrabbing(true);

        attracting = false;
        attractedObjRb = null;
    }
    #endregion

    #region OpenHand
    public void StartOpenHand()
    {
        EndGrab();
        EndAim();

        currentPose = Poses.OpenHand;
    }
    void OpenHand()
    {

    }
    #endregion

    public void StartSpellSelect()
    {
        EndAim();
        EndGrab();

        currentPose = Poses.SpellSelect;

        if (trailRenderer == null) return;

        trailRenderer.enabled = true;
    }
    void SpellSelect()
    {

    }
    void EndSpellSelect()
    {
        if (trailRenderer == null) return;

        trailRenderer.enabled = false;
    }

    public void EndPoses()
    {
        currentPose = Poses.Unknown;
        lineRenderer.enabled = false;

        EndGrab();
        EndSpellSelect();
        Invoke(nameof(EndAim), 2);
    }

}
