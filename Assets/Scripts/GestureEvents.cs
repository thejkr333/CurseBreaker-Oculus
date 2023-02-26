using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GestureEvents : MonoBehaviour
{
    [SerializeField] TMP_Text gestureText;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnGestureCompleted(GestureCompletionData data)
    {
        gestureText.gameObject.SetActive(true);
        gestureText.text = "Gesture finished \n Gesture recognized: " + data.gestureName + "\nGesture similarity: " + data.similarity;

        Debug.Log("Gesture finished");
        Debug.Log("Gesture recognized: " + data.gestureName);
        Debug.Log("Gesture similarity: " + data.similarity);

        Invoke(nameof(DisableText), 3);
    }

    void DisableText()
    {
        gestureText.gameObject.SetActive(false);
    }
}
