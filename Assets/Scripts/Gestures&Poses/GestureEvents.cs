using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GestureEvents : MonoBehaviour
{
    [SerializeField] List<string> gestureNames = new();

    [Header("HAND STUFF")]
    [SerializeField] GameObject leftHand, rightHand;
    [SerializeField] PoseEvents poseEventLeft, poseEventRight;
    OVRGrabber leftHandGrabber, rightHandGrabber;

    [SerializeField] GameObject currentDrawingHand;

    [SerializeField] GameObject fire, dark, light, water, air, earth;
    [SerializeField] Transform headPos;
    Vector3 initialPos;
    enum Gestures { Fire, Dark, Light, Water, Air, Earth}

    [Header("PORTAL")]
    [SerializeField] GameObject portalPrefab;

    private void Awake()
    {
        initialPos = headPos.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        poseEventLeft = GameObject.Find("LeftHandPoseEvents").GetComponent<PoseEvents>();
        poseEventRight = GameObject.Find("RightHandPoseEvents").GetComponent<PoseEvents>();
        leftHandGrabber = leftHand.GetComponent<OVRGrabber>();
        rightHandGrabber = leftHand.GetComponent<OVRGrabber>();
    }

    // Update is called once per frame
    void Update()
    {
       if(poseEventLeft.DrawingWithThisHand == true)
        {
            currentDrawingHand = leftHand;
        }
        if (poseEventRight.DrawingWithThisHand == true)
        {
            currentDrawingHand = rightHand;
        }
    }

    public void SetUpGestureNames(GestureRecognition gr)
    {
        int num_gestures = gr.numberOfGestures();
        for (int i = 0; i < num_gestures; i++)
        {
            string gesture_name = gr.getGestureName(i);
            gestureNames.Add(gesture_name);
        }
    }

    public void OnGestureCompleted(GestureCompletionData data)
    {
        Debug.Log("Gesture finished");
        Debug.Log("Gesture recognized: " + data.gestureName);
        Debug.Log("Gesture similarity: " + data.similarity);

        if(data.similarity < 0.4f) return;

        switch(data.gestureName)
        {
            case "fire":
                Fire();
                break;
            case "dark":
                Dark();
                break;
            case "light":
                Light();
                break;
            case "water":
                Water();
                break;
            case "air":
                Air();
                break;
            case "earth":
                Earth();
                break;
            case "portal":
                CreatePortal();
                break;
            default:
                Debug.Log("Gesture not assigned");
                break;
        }
    }

    #region gesture Elements
    void Fire()
    {
        GameObject clon = Instantiate(fire);
        clon.transform.position = currentDrawingHand.transform.position;
        //clon.transform.forward = headPos.forward;
        //clon.transform.position = new Vector3(headPos.position.x, (headPos.position.y - initialPos.y) / 3 * 2, headPos.position.z);
        //clon.GetComponent<Rigidbody>().velocity = clon.transform.forward * 5;
        Debug.Log("Fire");
    }
    private void Dark()
    {
        GameObject clon = Instantiate(dark);
        clon.transform.position = currentDrawingHand.transform.position;
        /*clon.transform.forward = headPos.forward;
        clon.transform.position = new Vector3(headPos.position.x, (headPos.position.y - initialPos.y) / 3 * 2, headPos.position.z);
        clon.GetComponent<Rigidbody>().velocity = clon.transform.forward * 5; */
        Debug.Log("Dark");
    }
    private void Light()
    {
        GameObject clon = Instantiate(light);
        clon.transform.position = currentDrawingHand.transform.position;/*
        clon.transform.forward = headPos.forward;
        clon.transform.position = new Vector3(headPos.position.x, (headPos.position.y - initialPos.y) / 3 * 2, headPos.position.z);
        clon.GetComponent<Rigidbody>().velocity = clon.transform.forward * 5;*/
        Debug.Log("Light");
    }
    private void Water()
    {
        GameObject clon = Instantiate(water);
        clon.transform.position = currentDrawingHand.transform.position;

        /*
        clon.transform.forward = headPos.forward;
        clon.transform.position = new Vector3(headPos.position.x, (headPos.position.y - initialPos.y) / 3 * 2, headPos.position.z);
        clon.GetComponent<Rigidbody>().velocity = clon.transform.forward * 5;*/
        Debug.Log("Water");
    }
    private void Air()
    {
        GameObject clon = Instantiate(air);
        clon.transform.position = currentDrawingHand.transform.position;
        /*
        clon.transform.forward = headPos.forward;
        clon.transform.position = new Vector3(headPos.position.x, (headPos.position.y - initialPos.y) / 3 * 2, headPos.position.z);
        clon.GetComponent<Rigidbody>().velocity = clon.transform.forward * 5;*/
        Debug.Log("Air");
    }
    private void Earth()
    {
        GameObject clon = Instantiate(earth);
        clon.transform.position = currentDrawingHand.transform.position;
        /*
        clon.transform.forward = headPos.forward;
        clon.transform.position = new Vector3(headPos.position.x, (headPos.position.y - initialPos.y) / 3 * 2, headPos.position.z);
        clon.GetComponent<Rigidbody>().velocity = clon.transform.forward * 5;*/
        Debug.Log("Earth");
    }
    #endregion
    void CreatePortal()
    {
        GameObject clon = Instantiate(portalPrefab);
        clon.transform.position = new Vector3(headPos.position.x, headPos.position.y - .5f, headPos.position.z + 2);
    }
}
