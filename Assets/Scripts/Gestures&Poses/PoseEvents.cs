using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;

public class PoseEvents : MonoBehaviour
{
    [Header("GENERAL VARIABLES")]
    [SerializeField] PoseEvents otherHandPoseEvent;
    [SerializeField] bool mainHand;
    public bool DrawingWithThisHand;
    public enum Poses { Aiming, Grab, OpenHand, SpellSelect, TV, Unknown }
    public Poses CurrentPose;

    //Dictionary<Poses, bool> pose = new();

    [SerializeField] protected OVRSkeleton handSkeleton;
    PoseGrab poseGrab;

    protected List<OVRBone> fingerbones = null;

    private bool hasStarted = false;


    [Header("AIMING")]
    Vector3 indexProximal = Vector3.zero;
    Vector3 indexTip = Vector3.zero;
    Vector3 thumbMetacarpal = Vector3.zero;
    private Color blue, white;
    LineRenderer lineRenderer;


    [Header("TV")]
    [SerializeField] GameObject hiddenGO;
    [SerializeField] Transform head;


    [Header("GRABBING")]
    [SerializeField] bool attracting = false;
    [SerializeField] LayerMask interactable, grabbed;
    Outline lastOutline;
    Transform grabPoint;
    Rigidbody attractedObjRb;
    LayerMask objectLayer;


    [Header("DRAWING GESTURES")]
    [SerializeField] Material paintMaterial;
    public bool recordingGesture;
    TrailRenderer trailRenderer;
    GameObject drawingFingerTip;
    void Start()
    {
        poseGrab = handSkeleton.GetComponent<PoseGrab>();
        lineRenderer = GetComponent<LineRenderer>();
        hiddenGO.SetActive(false);
        lineRenderer.enabled = false;

        //Defining colours and alpha for the line here
        //it is only able to understand it as an array, so to change it, it needs to be lerped individuallly
        /*Blue = new GradientColorKey[2];
        Blue[0].color = Color.blue;
        Blue[0].time = 0; 
        Blue[1].color = Color.blue;
        Blue[1].time = 1;
        White = new GradientColorKey[2];
        White[0].color = Color.white;
        White[0].time = 0;
        White[1].color = Color.white;
        White[1].time = 1;
        Alpha = new GradientAlphaKey[2];
        Alpha[0].alpha = 1;
        Alpha[0].time = 0;
        Alpha[1].alpha = 1;
        Alpha[1].time = 1;
        */
        blue = Color.blue;
        white = Color.white;

        grabPoint = handSkeleton.transform.GetChild(0);
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
                drawingFingerTip = bone.Transform.gameObject;

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

        switch (CurrentPose)
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
    private void FixedUpdate()
    {
        if (attracting)
        {
            AttractFixed();
        }
    }

    #region Aim
    public void StartAim()
    {
        if (CurrentPose != Poses.Aiming) StartNewPose(CurrentPose);
        else return;

        CurrentPose = Poses.Aiming;

        //Testing the grab end on another pose
        DrawingWithThisHand = false;

        EndGrab();

        //Colour Reset every time you start aiming, just incase it doesnt. at best the line should be blue. might also not change to white as the gradient itself cannot be lerped...
        //Blue[0].color = Color.blue;
        //Blue[1].color = Color.blue;
        //lineRenderer.colorGradient.SetKeys(Blue, Alpha);
        lineRenderer.material.color = blue;      
    }
    void Aim()
    {
        lineRenderer.enabled = true;
        indexProximal = Vector3.zero;
        indexTip = Vector3.zero;

        //Colour Change goes in these two lines, just changing the start and end values towards white over 5 seconds
        Color.Lerp(lineRenderer.material.color, white, 5);
      //  Color.Lerp(Blue[0].color, White[0].color, 5);
     //   Color.Lerp(Blue[1].color, White[1].color, 5);

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

            //you outline the new object
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
        if (CurrentPose != Poses.Grab) StartNewPose(CurrentPose);
        else return;

        CurrentPose = Poses.Grab;
        DrawingWithThisHand = false;
    }
    void Grab()
    {
        if (attracting) return;

        if (lastOutline != null && lastOutline.enabled == true)
        {
            Rigidbody rb = lastOutline.GetComponent<Rigidbody>();
            if (rb == null) return;

            attractedObjRb = rb;
            if (rb.GetComponent<StirringStick>() != null) rb.GetComponent<StirringStick>().DisableAnim();
            DeselectAim();
            StartAttract();
            //StartCoroutine(AttractObject());
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
            Vector3 direction = (grabPoint.position - obj.transform.position).normalized;
            if (attractedObjRb != null) attractedObjRb.AddForce(direction * 3, ForceMode.Force);

            yield return 0;
        }

        attractedObjRb.velocity = Vector3.zero;
        attractedObjRb.transform.position = handSkeleton.transform.position;
        obj.layer = objectLayer;
        objectLayer = 0;
        attractedObjRb.useGravity = true;

        poseGrab.DetectGrabbing(true);

        attracting = false;
        attractedObjRb = null;
    }
    void StartAttract()
    {
        EndAim();
        poseGrab.DetectGrabbing(false);

        attractedObjRb.useGravity = false;

        objectLayer = attractedObjRb.gameObject.layer;
        int LayerGrabbed = LayerMask.NameToLayer("Grabbed");
        attractedObjRb.gameObject.layer = LayerGrabbed;

        attracting = true;
    }
    void AttractFixed()
    {
        if (Vector3.Distance(attractedObjRb.gameObject.transform.position, handSkeleton.transform.position) > .2f)
        {
            Vector3 direction = (handSkeleton.transform.position - attractedObjRb.gameObject.transform.position).normalized;
            if (attractedObjRb != null) attractedObjRb.AddForce(direction * 3, ForceMode.Force);
        }
        else
        {
            attractedObjRb.velocity = Vector3.zero;
            attractedObjRb.transform.position = handSkeleton.transform.position;
            attractedObjRb.gameObject.layer = objectLayer;
            objectLayer = 0;
            attractedObjRb.useGravity = true;

            poseGrab.DetectGrabbing(true);

            attracting = false;
            attractedObjRb = null;
        }
    }
    #endregion

    #region OpenHand
    public void StartOpenHand()
    {
        if (CurrentPose != Poses.OpenHand) StartNewPose(CurrentPose);
        else return;

        CurrentPose = Poses.OpenHand;
    }
    void OpenHand()
    {
        EndGrab();
    }

    void EndOpenHand()
    {

    }
    #endregion

    #region SpellSelect
    public void StartSpellSelect()
    {
        if (CurrentPose != Poses.SpellSelect) StartNewPose(CurrentPose);
        else return;

        CurrentPose = Poses.SpellSelect;

        AudioManager.Instance.PlaySoundDynamic("magic_drawing", drawingFingerTip.gameObject);

        EndGrab();
        DrawingWithThisHand = true;

        if (trailRenderer == null) return;

        trailRenderer.enabled = true;
    }
    void SpellSelect()
    {
        recordingGesture = true;
    }
    void EndSpellSelect()
    {
        AudioManager.Instance.StopSound("magic_drawing", drawingFingerTip.gameObject);

        recordingGesture = false;

        if (trailRenderer == null) return;

        trailRenderer.Clear();
        trailRenderer.enabled = false;
    }
    #endregion

    #region TV
    public void StartTV()
    {

        
        if (CurrentPose != Poses.TV) StartNewPose(CurrentPose);

        CurrentPose = Poses.TV;
    }

    void TV()
    {
        if (otherHandPoseEvent.CurrentPose != Poses.TV) return;

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
               // EndGrab();             
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
                //EndGrab();
                EndTV();
                EndSpellSelect();
                EndOpenHand();
                break;
        }
    }

    public void EndPoses()
    {
        CurrentPose = Poses.Unknown;

        StartNewPose(CurrentPose);
    }
}
