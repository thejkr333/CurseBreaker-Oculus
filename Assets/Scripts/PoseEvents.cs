using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;

public class PoseEvents : MonoBehaviour
{
    [SerializeField] PoseEvents otherHandPoseEvent;
    [SerializeField] bool mainHand;

    public enum Poses { Aiming, Grab, OpenHand, SpellSelect, TV, Unknown }
    public Poses currentPose;

    Dictionary<Poses, bool> pose = new();

    [SerializeField] protected OVRSkeleton handSkeleton;
    PoseGrab poseGrab;
    LineRenderer lineRenderer;
    TrailRenderer trailRenderer;
    protected List<OVRBone> fingerbones = null;
    [SerializeField] LayerMask interactable, grabbed;

    private bool hasStarted = false;
    [SerializeField] bool attracting = false;

    //bones for aiming
    Vector3 indexProximal = Vector3.zero;
    Vector3 indexTip = Vector3.zero;
    public Vector3 thumbMetacarpal = Vector3.zero;
    [SerializeField] Material paintMaterial;
    [SerializeField] GameObject hiddenGO;
    [SerializeField] Transform head;

    [SerializeField] Outline lastOutline;
    Rigidbody attractedObjRb;
    LayerMask objectLayer;

    public bool recordingGesture;
    void Start()
    {
        poseGrab = handSkeleton.GetComponent<PoseGrab>();
        lineRenderer = GetComponent<LineRenderer>();
        hiddenGO.SetActive(false);
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
            if (bone.Id == OVRSkeleton.BoneId.Hand_MiddleTip)
            {
                trailRenderer = bone.Transform.gameObject.AddComponent<TrailRenderer>();
                trailRenderer.material = paintMaterial;
                paintMaterial.color = Color.white;
                trailRenderer.time = 4;
                trailRenderer.minVertexDistance = .0001f;
                trailRenderer.startWidth = .01f;
                trailRenderer.enabled = false;
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

            case Poses.TV:
                TV();
                break;

            case Poses.Unknown:
                break;
        }
    }

    #region Aim
    public void StartAim()
    {
        if (currentPose != Poses.Aiming) StartNewPose(currentPose);

        currentPose = Poses.Aiming;
    }
    void Aim()
    {
        lineRenderer.enabled = true;
        indexProximal = Vector3.zero;
        indexTip = Vector3.zero;

        foreach (OVRBone bone in fingerbones)
        {
            if (bone.Id == OVRSkeleton.BoneId.Hand_WristRoot)
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
        Invoke(nameof(DeselectAim), 2);       
    }
    void DeselectAim()
    {
        if (lastOutline != null && lastOutline.enabled) lastOutline.enabled = false;
        lastOutline = null;
    }
    #endregion Aim

    #region Grab
    public void StartGrab()
    {
        if (currentPose != Poses.Grab) StartNewPose(currentPose);

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
            DeselectAim();
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

            if (attractedObjRb.TryGetComponent<AlwaysLookToCam>(out AlwaysLookToCam lookToCam))
            {
                lookToCam.enabled = false;
            }
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

        while (Vector3.Distance(obj.transform.position, handSkeleton.transform.position) > .2f)
        {
            if (attractedObjRb != null) attractedObjRb.AddForce((handSkeleton.transform.position - obj.transform.position).normalized * 3);

            yield return 0;
        }

        attractedObjRb.velocity = Vector3.zero;
        attractedObjRb.transform.position = handSkeleton.transform.position;
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
        if (currentPose != Poses.OpenHand) StartNewPose(currentPose);

        currentPose = Poses.OpenHand;
    }
    void OpenHand()
    {

    }

    void EndOpenHand()
    {

    }
    #endregion

    #region SpellSelect
    public void StartSpellSelect()
    {
        if (currentPose != Poses.SpellSelect) StartNewPose(currentPose);

        currentPose = Poses.SpellSelect;

        if (trailRenderer == null) return;

        trailRenderer.enabled = true;
    }
    void SpellSelect()
    {
        recordingGesture = true;
    }
    void EndSpellSelect()
    {
        recordingGesture = false;

        if (trailRenderer == null) return;

        trailRenderer.Clear();
        trailRenderer.enabled = false;
    }
    #endregion

    #region TV
    public void StartTV()
    {
        if (currentPose != Poses.TV) StartNewPose(currentPose);

        currentPose = Poses.TV;
    }

    void TV()
    {
        if (otherHandPoseEvent.currentPose != Poses.TV) return;

        Vector3 localThumbCoords = Vector3.zero;
        foreach (OVRBone bone in fingerbones)
        {
            if (bone.Id == OVRSkeleton.BoneId.Hand_Thumb0)
            {
                //Global
                thumbMetacarpal = bone.Transform.position;
                break;
            }
        }

        if (mainHand)
        {
            float x = (thumbMetacarpal.x - otherHandPoseEvent.thumbMetacarpal.x) * 0.5f;
            float y = (thumbMetacarpal.y - otherHandPoseEvent.thumbMetacarpal.y) * 0.5f;
            float z = (thumbMetacarpal.z - otherHandPoseEvent.thumbMetacarpal.z) * 0.5f;

            Vector3 centre = new Vector3(thumbMetacarpal.x - x, thumbMetacarpal.y - y, thumbMetacarpal.z - z);
            hiddenGO.transform.position = centre;
            //Renderer rend = hiddenGO.GetComponentInChildren<Renderer>();

            hiddenGO.transform.forward = (centre - head.position).normalized;
            hiddenGO.transform.localScale = Vector3.one;
            localThumbCoords = hiddenGO.transform.worldToLocalMatrix.MultiplyPoint(thumbMetacarpal);
            hiddenGO.transform.localScale = new Vector3(Mathf.Abs(localThumbCoords.x) * 2, Mathf.Abs(localThumbCoords.y) * 2, transform.localScale.z);

            //Debug.Log("plane right x: " + (centre.x + rend.bounds.extents.x) + " vs right thumb: " + thumbMetacarpal.x);
            //Debug.Log("plane up y: " + (centre.y + rend.bounds.extents.y) + " vs right thumb: " + thumbMetacarpal.y);

            //Vector3 centreAlignedWithHand = centre;
            //centreAlignedWithHand.y = thumbMetacarpal.y;

            //float distanceProjectedVector = Vector3.Distance(centreAlignedWithHand, thumbMetacarpal);

            //if (rend.bounds.extents.x - distanceProjectedVector  > .05f)
            //{
            //    Resize(true, false);
            //}
            //else if (rend.bounds.extents.x - distanceProjectedVector < -.05f )
            //{
            //    Resize(true, true);
            //}
            //if (centre.y + rend.bounds.extents.y - thumbMetacarpal.y > .05f)
            //{
            //    Resize(false, false);
            //}
            //else if (centre.y + rend.bounds.extents.y - thumbMetacarpal.y < -.05f)
            //{
            //    Resize(false, true);
            //}

            hiddenGO.SetActive(true);
        }
    }

    void Resize(bool XAxis, bool increasing)
    {
        Transform tr = hiddenGO.transform;

        if (XAxis)
        {
            if (increasing)
            {
                tr.localScale += new Vector3(.01f, 0, 0);
            }
            else
            {
                tr.localScale -= new Vector3(.01f, 0, 0);
            }
        }
        else
        {
            if (increasing)
            {
                tr.localScale += new Vector3(0, .01f, 0);
            }
            else
            {
                tr.localScale -= new Vector3(0, .01f, 0);
            }
        }

        //Clamp values so it's never too small or too big
        if (tr.localScale.x < .1f) tr.localScale = new Vector3(.1f, tr.localScale.y, tr.localScale.z);
        if (tr.localScale.y < .1f) tr.localScale = new Vector3(tr.localScale.x, .1f, tr.localScale.z);
        if (tr.localScale.x > .6f) tr.localScale = new Vector3(.6f, tr.localScale.y, tr.localScale.z);
        if (tr.localScale.y > .6f) tr.localScale = new Vector3(tr.localScale.x, .6f, tr.localScale.z);
    }
    void EndTV()
    {
        hiddenGO.SetActive(false);
    }
    #endregion
    void StartNewPose(Poses lastPose)
    {
        switch (lastPose)
        {
            case Poses.Aiming:
                EndAim();
                break;

            case Poses.Grab:
                EndGrab();
                break;

            case Poses.OpenHand:
                EndOpenHand();
                break;

            case Poses.TV:
                EndTV();
                break;

            case Poses.SpellSelect:
                EndSpellSelect();
                break;

            case Poses.Unknown:
                EndAim();
                EndGrab();
                EndTV();
                EndSpellSelect();
                EndOpenHand();
                break;
        }
    }

    public void EndPoses()
    {
        currentPose = Poses.Unknown;

        StartNewPose(currentPose);
    }
}
