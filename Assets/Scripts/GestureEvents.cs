using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GestureEvents : MonoBehaviour
{
    [SerializeField] TMP_Text gestureText;

    [SerializeField] List<string> gestureNames = new();

    enum Gestures { Fire, Dark, Light, Water, Air, Earth}
    // Start is called before the first frame update
    void Start()
    {

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
        Debug.Log("Fire");
    }
    private void Dark()
    {
        Debug.Log("Dark");
    }
    private void Light()
    {
        Debug.Log("Light");

    }
    private void Water()
    {
        Debug.Log("Water");

    }
    private void Air()
    {
        Debug.Log("Air");

    }
    private void Earth()
    {
        Debug.Log("Earth");

    }
    #endregion
}
