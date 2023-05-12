using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;
using Oculus.Interaction;

public class PoseEvents : MonoBehaviour
{
    [Header("GENERAL VARIABLES")]
    [SerializeField] PoseEvents otherHandPoseEvent;
    [SerializeField] bool mainHand;
    public bool DrawingWithThisHand;
    public enum Poses { Aiming, Grab, OpenHand, SpellSelect, TV, Unknown }
    public Poses CurrentPose;

    public OVRSkeleton HandSkeleton;
    OVRHand hand;
    PoseGrab poseGrab;
    bool tracking;

    protected List<OVRBone> fingerbones = null;

    private bool hasStarted = false;

    [Header("AIMING")]
    Vector3 indexProximal = Vector3.zero;
    Vector3 indexTip = Vector3.zero;
    Vector3 thumbMetacarpal = Vector3.zero;
    private Color blue, white;
    LineRenderer lineRenderer;
    LineController lineController;


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
    [SerializeField] float objMaxDistanceMovement;
    bool kinematic;


    [Header("DRAWING GESTURES")]
    [SerializeField] Material paintMaterial;
    public bool recordingGesture;
    TrailRenderer trailRenderer;
    GameObject drawingFingerTip;


    [Header("OPENHAND")]
    [SerializeField] bool hasClapped;
    public static Action Clap;
    void Start()
    {
        poseGrab = HandSkeleton.GetComponent<PoseGrab>();
        hand = HandSkeleton.GetComponent<OVRHand>();
        lineController = GetComponent<LineController>();
        lineRenderer = GetComponent<LineRenderer>();
        hiddenGO.SetActive(false);
        lineController.enabled = false;
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

        grabPoint = HandSkeleton.transform.GetChild(0);
        // When the Oculus hand had his time to initialize hand, with a simple coroutine i start a delay of
        // a function to initialize the script
        StartCoroutine(DelayRoutine(Initialize));
    }
    // Coroutine used for delay some function
    public IEnumerator DelayRoutine(Action actionToDo)
    {
        while (!HandSkeleton.IsInitialized)
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
        fingerbones = new List<OVRBone>(HandSkeleton.Bones);
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
        if (!hand.IsTracked) return;

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
        //lineRenderer.material.color = blue;

        lineController.enabled = true;
        lineRenderer.enabled = true;
    }
    void Aim()
    {
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
            Outline outline = null;

            if (hitInfo.transform.TryGetComponent(out Bubble bubble))
            {
                outline = bubble.GetComponentInChildren<Outline>();
            }
            else if (!hitInfo.transform.TryGetComponent(out outline)) return;

            //you outline the new object
            outline.enabled = true;
            PointingParticlesManager.Instance.NewObjectOutlined(HandSkeleton.gameObject, outline.gameObject);

            if (lastOutline != null && lastOutline != outline && lastOutline.enabled) { lastOutline.enabled = false; lastOutline = null; }

            lastOutline = outline;
        }
    }
    void EndAim()
    {
        lineController.enabled = false;
        lineRenderer.enabled = false;
        Invoke(nameof(DeselectAim), 2);
    }
    void DeselectAim()
    {
        if (lastOutline != null && lastOutline.enabled) lastOutline.enabled = false;
        lastOutline = null;
        PointingParticlesManager.Instance.StopEmitting();
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

    private void GrabDependingOnObject(ref Rigidbody attractedObjRb)
    {
        if (attractedObjRb.TryGetComponent(out StirringStick stick)) stick.DisableAnim();

        //If it's a chest spawn an ingredient and attract it
        else if (attractedObjRb.TryGetComponent(out IngredientChest ingredientChest))
        {
            GameObject ing = ingredientChest.InstantiateIngredient();
            if (ing == null)
            {
                //no money
                return;
            }
            else attractedObjRb = ing.GetComponent<Rigidbody>();
        }
        else if (attractedObjRb.TryGetComponent(out DecorationObject decorObj))
        {
            decorObj.StartGrabbing();
        }
        else if(attractedObjRb.TryGetComponent(out Ingredient ingredient))
        {
            if (ingredient.transform != null)
            {
                Bubble _bubble = ingredient.GetComponentInParent<Bubble>();
                _bubble.Pop();
                attractedObjRb.transform.SetParent(null);
            }

            attractedObjRb.isKinematic = false;
        }
    }

    void Grab()
    {
        if (attracting) return;

        if (lastOutline != null && lastOutline.enabled == true)
        {
            if (!lastOutline.TryGetComponent(out Rigidbody rb)) return;

            attractedObjRb = rb;

            GrabDependingOnObject(ref attractedObjRb);
            DeselectAim();
            StartAttract();
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
            attracting = false;

            if (attractedObjRb.TryGetComponent<AlwaysLookToCam>(out AlwaysLookToCam lookToCam))
            {
                lookToCam.enabled = false;
            }

            attractedObjRb.useGravity = true;
            int _layerInteractable = LayerMask.NameToLayer("Interactable");
            attractedObjRb.gameObject.layer = _layerInteractable;
            attractedObjRb = null;
        }

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

        while (Vector3.Distance(obj.transform.position, HandSkeleton.transform.position) > .2f)
        {
            Vector3 direction = (grabPoint.position - obj.transform.position).normalized;
            if (attractedObjRb != null) attractedObjRb.AddForce(direction * 3, ForceMode.Force);

            yield return 0;
        }

        attractedObjRb.velocity = Vector3.zero;
        attractedObjRb.transform.position = HandSkeleton.transform.position;
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
        int _layerGrabbed = LayerMask.NameToLayer("Grabbed");
        attractedObjRb.gameObject.layer = _layerGrabbed;
        kinematic = attractedObjRb.isKinematic;

        attracting = true;
    }
    void AttractFixed()
    {
        GameObject _obj = attractedObjRb.gameObject;
        attractedObjRb.isKinematic = false;

        if (Vector3.Distance(_obj.transform.position, HandSkeleton.transform.position) > .2f)
        {
            //Vector3 _direction = (HandSkeleton.transform.position - _obj.transform.position).normalized;
            //attractedObjRb.AddForce(_direction * 3, ForceMode.Force);

            _obj.transform.position = Vector3.MoveTowards(_obj.transform.position, grabPoint.position, objMaxDistanceMovement);
        }
        else
        {
            int _layerInteractable = LayerMask.NameToLayer("Interactable");
            _obj.layer = _layerInteractable;

            attractedObjRb.velocity = Vector3.zero;
            attractedObjRb.transform.position = HandSkeleton.transform.GetChild(0).position;
            attractedObjRb.gameObject.layer = objectLayer;
            objectLayer = 0;
            attractedObjRb.useGravity = true;
            attractedObjRb.isKinematic = kinematic;

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
        hasClapped = false;
    }
    void OpenHand()
    {
        EndGrab();

        if (otherHandPoseEvent.CurrentPose != Poses.OpenHand) return;

        if (mainHand)
        {
            //Debug.LogWarning("distance: " + Vector3.Distance(otherHandPoseEvent.HandSkeleton.transform.position, HandSkeleton.transform.position));

            //Check if hands are close, if close put clap to true and wait until they get away
            if (Vector3.Distance(otherHandPoseEvent.HandSkeleton.transform.position, HandSkeleton.transform.position) < .1f)
            {
                if (hasClapped) return;

                hasClapped = true;
                Clap?.Invoke();
                Debug.LogWarning("Clap");
            }
            else if (Vector3.Distance(otherHandPoseEvent.HandSkeleton.transform.position, HandSkeleton.transform.position) > .2f) hasClapped = false;
        }
    }

    void EndOpenHand()
    {
        hasClapped = false;
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

        Vector3 _localThumbCoords = Vector3.zero;
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
            float _x = (thumbMetacarpal.x - otherHandPoseEvent.thumbMetacarpal.x) * 0.5f;
            float _y = (thumbMetacarpal.y - otherHandPoseEvent.thumbMetacarpal.y) * 0.5f;
            float _z = (thumbMetacarpal.z - otherHandPoseEvent.thumbMetacarpal.z) * 0.5f;

            Vector3 _centre = new Vector3(thumbMetacarpal.x - _x, thumbMetacarpal.y - _y, thumbMetacarpal.z - _z);
            hiddenGO.transform.position = _centre;
            //Renderer rend = hiddenGO.GetComponentInChildren<Renderer>();

            hiddenGO.transform.forward = (_centre - head.position).normalized;
            hiddenGO.transform.localScale = Vector3.one;
            _localThumbCoords = hiddenGO.transform.worldToLocalMatrix.MultiplyPoint(thumbMetacarpal);
            hiddenGO.transform.localScale = new Vector3(Mathf.Abs(_localThumbCoords.x) * 2, Mathf.Abs(_localThumbCoords.y) * 2, transform.localScale.z);

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
        Transform _tr = hiddenGO.transform;

        if (XAxis)
        {
            if (increasing)
            {
                _tr.localScale += new Vector3(.01f, 0, 0);
            }
            else
            {
                _tr.localScale -= new Vector3(.01f, 0, 0);
            }
        }
        else
        {
            if (increasing)
            {
                _tr.localScale += new Vector3(0, .01f, 0);
            }
            else
            {
                _tr.localScale -= new Vector3(0, .01f, 0);
            }
        }

        //Clamp values so it's never too small or too big
        if (_tr.localScale.x < .1f) _tr.localScale = new Vector3(.1f, _tr.localScale.y, _tr.localScale.z);
        if (_tr.localScale.y < .1f) _tr.localScale = new Vector3(_tr.localScale.x, .1f, _tr.localScale.z);
        if (_tr.localScale.x > .6f) _tr.localScale = new Vector3(.6f, _tr.localScale.y, _tr.localScale.z);
        if (_tr.localScale.y > .6f) _tr.localScale = new Vector3(_tr.localScale.x, .6f, _tr.localScale.z);
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
