using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GestureEvents : MonoBehaviour
{
    [SerializeField] TMP_Text gestureText;

    [SerializeField] List<string> gestureNames = new();

    [SerializeField] GameObject leftHand, rightHand;
    OVRGrabber leftHandGrabber, rightHandGrabber;

    [SerializeField] GameObject fire, dark, light, water, air, earth;
    [SerializeField] Transform headPos;
    Vector3 initialPos;
    enum Gestures { Fire, Dark, Light, Water, Air, Earth}

    private void Awake()
    {
        initialPos = headPos.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        leftHandGrabber = leftHand.GetComponent<OVRGrabber>();
        rightHandGrabber = leftHand.GetComponent<OVRGrabber>();
    }

    // Update is called once per frame
    void Update()
    {

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
        //gestureText.gameObject.SetActive(true);
        //gestureText.text = "Gesture finished \n Gesture recognized: " + data.gestureName + "\nGesture similarity: " + data.similarity;

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
            default:
                Debug.Log("Gesture not assigned");
                break;
        }

        //Invoke(nameof(DisableText), 3);
    }

    void DisableText()
    {
        gestureText.gameObject.SetActive(false);
    }

    #region gesture Methods
    void Fire()
    {
        GameObject clon = Instantiate(fire);
        clon.transform.forward = headPos.forward;
        clon.transform.position = new Vector3(headPos.position.x, (headPos.position.y - initialPos.y) / 3 * 2, headPos.position.z);
        clon.GetComponent<Rigidbody>().velocity = clon.transform.forward * 5;
        Debug.Log("Fire");
    }
    private void Dark()
    {
        GameObject clon = Instantiate(dark);
        clon.transform.forward = headPos.forward;
        clon.transform.position = new Vector3(headPos.position.x, (headPos.position.y - initialPos.y) / 3 * 2, headPos.position.z);
        clon.GetComponent<Rigidbody>().velocity = clon.transform.forward * 5;
        Debug.Log("Dark");
    }
    private void Light()
    {
        GameObject clon = Instantiate(light);
        clon.transform.forward = headPos.forward;
        clon.transform.position = new Vector3(headPos.position.x, (headPos.position.y - initialPos.y) / 3 * 2, headPos.position.z);
        clon.GetComponent<Rigidbody>().velocity = clon.transform.forward * 5;
        Debug.Log("Light");
    }
    private void Water()
    {
        GameObject clon = Instantiate(water);
        clon.transform.forward = headPos.forward;
        clon.transform.position = new Vector3(headPos.position.x, (headPos.position.y - initialPos.y) / 3 * 2, headPos.position.z);
        clon.GetComponent<Rigidbody>().velocity = clon.transform.forward * 5;
        Debug.Log("Water");
    }
    private void Air()
    {
        GameObject clon = Instantiate(air);
        clon.transform.forward = headPos.forward;
        clon.transform.position = new Vector3(headPos.position.x, (headPos.position.y - initialPos.y) / 3 * 2, headPos.position.z);
        clon.GetComponent<Rigidbody>().velocity = clon.transform.forward * 5;
        Debug.Log("Air");
    }
    private void Earth()
    {
        GameObject clon = Instantiate(earth);
        clon.transform.forward = headPos.forward;
        clon.transform.position = new Vector3(headPos.position.x, (headPos.position.y - initialPos.y) / 3 * 2, headPos.position.z);
        clon.GetComponent<Rigidbody>().velocity = clon.transform.forward * 5;
        Debug.Log("Earth");
    }
    #endregion
}
