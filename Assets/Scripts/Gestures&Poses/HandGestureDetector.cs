using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System.Collections;
using System;
using UnityEngine.SpatialTracking;
// struct = class without functions
[System.Serializable]
public struct Pose
{
    public string Name;
    public bool OneHand;
    public List<Vector3> FingerDatas;
    public UnityEvent OnRecognized;
}
public class HandGestureDetector : MonoBehaviour
{
    [SerializeField] TMP_Text poseText;

    // How much accurate the recognize should be
    [Header("Threshold value")]
    public float Threshold = 0.1f;
    // Add the component that refer to the skeleton hand ("OVRCustomHandPrefab_R" or "OVRCustomHandPrefab_L")
    [Header("Hand Skeleton")]
    public OVRSkeleton Skeleton;
    // List that will be populated after we save some gestures
    [Header("List of Gestures")]
    public List<Pose> Poses;
    // List of bones took from the OVRSkeleton
    private List<OVRBone> fingerbones = null;
    // Boolean for the debugMode duh!
    [Header("DebugMode")]
    public bool DebugMode = true;
    // Other boolean to check if are working correctly
    private bool hasStarted = false;
    private bool hasRecognize = false;
    private bool done = false;
    // Add an event if you want to make happen when a gesture is not identified
    [Header("Not Recognized Event")]
    public UnityEvent NotRecognize;
    void Start()
    {
        // When the Oculus hand had his time to initialize hand, with a simple coroutine i start a delay of
        // a function to initialize the script
        StartCoroutine(DelayRoutine(Initialize));
    }
    // Coroutine used for delay some function
    public IEnumerator DelayRoutine(Action actionToDo)
    {
        while (!Skeleton.IsInitialized)
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
        fingerbones = new List<OVRBone>(Skeleton.Bones);
    }

    void Update()
    {
        // if in debug mode and we press Space, we will save a gesture
        if (DebugMode && Input.GetKeyDown(KeyCode.Space))
        {
            // Call the function for save the gesture
            Save();
        }

        //if the initialization was successful
        if (hasStarted)
        {
            // start to Recognize every gesture we make
            Pose _currentGesture = Recognize();

            // we will associate the recognize to a boolean to see if the Gesture
            // we are going to make is one of the gesture we already saved
            hasRecognize = !_currentGesture.Equals(new Pose());

            // and if the gesture is recognized
            if (hasRecognize)
            {
                // we change another boolean to avoid a loop of event
                done = true;
                //poseText.text = currentGesture.name;
                // after that i will invoke what put in the Event if is present
                _currentGesture.OnRecognized?.Invoke();
            }
            // if the gesture we done is no more recognized
            else
            {
                // and we just activated the boolean from earlier
                if (done)
                {
                    Debug.Log("Not Recognized");
                    // we set to false the boolean again, so this will not loop
                    done = false;

                    // and finally we will invoke an event when we end to make the previous gesture
                    NotRecognize?.Invoke();
                }
            }
        }
    }

    void Save()
    {
        // We create a new Gesture struct
        Pose _pose = new Pose();

        // givin to it a default name
        _pose.Name = "New Gesture";

        // we create also a new list of Vector 3
        List<Vector3> data = new List<Vector3>();

        // with a foreach we go through every bone we set at the start
        // in the list of fingerbones
        foreach (var bone in fingerbones)
        {
            // and we will going to populate the list of Vector3 we done before with all the trasform found in the fingerbones
            // the fingers positions are in base at the hand Root
            data.Add(Skeleton.transform.InverseTransformPoint(bone.Transform.position));
        }

        // after the foreach we are going to associate the list of Vector3 to the one we create from the struct "g"
        _pose.FingerDatas = data;

        // and in the end we will going to add this new gesture in our list of gestures
        Poses.Add(_pose);
    }

    Pose Recognize()
    {
        // in the Update if we initialized correctly, we create a new Gesture
        Pose _currentGesture = new Pose();

        // we set a new float of a positive infinity
        float _currentMin = Mathf.Infinity;

        // we start a foreach loop inside our list of gesture
        foreach (var gesture in Poses)
        {
            // initialize a new float about the distance
            float _sumDistance = 0;

            // and a new bool to check if discart a gesture or not
            bool _isDiscarded = false;

            // then with a for loop we check inside the list of bones we initalized at the start with "SetSkeleton"
            for (int i = 0; i < fingerbones.Count; i++)
            {
                // then we create a Vector3 that is exactly the transform from global position to local position of the current hand
                // we are making the gesture
                Vector3 _currentData = Skeleton.transform.InverseTransformPoint(fingerbones[i].Transform.position);

                // with a new float we calculate the distance between the current gesture we are making with all the gesture we saved
                float _distance = Vector3.Distance(_currentData, gesture.FingerDatas[i]);

                // if the distance is bigger respect the threshold
                if (_distance > Threshold)
                {
                    // then we discart it because or is another gesture or we made bad the gesture we wanted to do
                    _isDiscarded = true;
                    break;
                }

                // if the distance is correct we will add it to the first float we have created
                _sumDistance += _distance;
            }

            // if the gesture we made is not discarted and the distance of the gesture i minor then then Mathf.inifinty
            if (!_isDiscarded && _sumDistance < _currentMin)
            {
                // then we set current min to the distance we have
                _currentMin = _sumDistance;

                // and we associate the correct gesture we have just done to the variable we created
                _currentGesture = gesture;
            }
        }

        // so in the end we can return from the function the exact gesture we want to do
        return _currentGesture;
    }
}