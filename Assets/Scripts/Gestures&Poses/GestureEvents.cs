using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GestureEvents : MonoBehaviour
{
    [SerializeField] List<string> gestureNames = new();

    [Header("HAND STUFF")]
    [SerializeField] GameObject leftHand;
    [SerializeField] GameObject rightHand;
    [SerializeField] PoseEvents poseEventLeft, poseEventRight;
    GameObject currentDrawingHand;
    [SerializeField] Transform headPos;

    [Header("GESTURE ELEMENTS")]
    [SerializeField] GameObject fire, dark, light, water, air, earth, voidElement, garbo;
    enum Gestures { Fire, Dark, Light, Water, Air, Earth, Void, Garbo }

    [Header("PORTAL")]
    [SerializeField] GameObject portalPrefab;

    private void Start()
    {
        currentDrawingHand = rightHand;
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
        if (data.similarity < 0.4f) return;

        Debug.Log("Gesture finished");
        Debug.Log("Gesture recognized: " + data.gestureName);
        Debug.Log("Gesture similarity: " + data.similarity);

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
            case "void":
                VoidSpell();
                break;
//            case "garbo":
//                Debug.Log("Garbo recognised");
//                break;
            case "portal":
                CreatePortal();
                break;
            default:
                Garbo();
                Debug.Log("Gesture not recognised, using Garbo instead.");//"Gesture not assigned/recognised");
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
    private void VoidSpell()
    {
        GameObject clon = Instantiate(voidElement);
        clon.transform.position = currentDrawingHand.transform.position;
        /*
        clon.transform.forward = headPos.forward;
        clon.transform.position = new Vector3(headPos.position.x, (headPos.position.y - initialPos.y) / 3 * 2, headPos.position.z);
        clon.GetComponent<Rigidbody>().velocity = clon.transform.forward * 5;*/
        Debug.Log("Void");
    }
    private void Garbo()
    {
        GameObject clon = Instantiate(garbo);
        clon.transform.position = currentDrawingHand.transform.position;
        /*
        clon.transform.forward = headPos.forward;
        clon.transform.position = new Vector3(headPos.position.x, (headPos.position.y - initialPos.y) / 3 * 2, headPos.position.z);
        clon.GetComponent<Rigidbody>().velocity = clon.transform.forward * 5;*/
        Debug.Log("Garbo");
    }
    #endregion
    void CreatePortal()
    {
        GameObject clon = Instantiate(portalPrefab);
        clon.transform.position = new Vector3(headPos.position.x, headPos.position.y - .5f, headPos.position.z + 2);
    }
}
