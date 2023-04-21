using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Oculus;
//using Oculus.VR;
//using Oculus.Interaction;
//using Oculus.Interaction.OVR;

public class MenuFunctionality : MonoBehaviour
{
    public GameObject Menu;// HeightUp,HeightDown;
    // Start is called before the first frame update
    void Awake()
    {
        if (Menu == null)
        {
            Debug.LogError("Menu isn't set. Please set the canvas child of " + this.name + " as the menu in the inspector.");
        }
        Menu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Start) && (OVRInput.GetActiveController() == OVRInput.Controller.Hands))
        {
            Menu.SetActive(!Menu.activeInHierarchy);
            GameManager.Instance.InMenu = Menu.activeInHierarchy;
            // HeightUp.SetActive(!Menu.activeInHierarchy);
           // HeightDown.SetActive(!Menu.activeInHierarchy);
        }
        //Debug Keyboard Testing Uncomment for testing without headset
       /* if (Input.GetKeyDown(KeyCode.Space))
        {

            Menu.SetActive(!Menu.activeInHierarchy);
            HeightUp.SetActive(!Menu.activeInHierarchy);
            HeightDown.SetActive(!Menu.activeInHierarchy);
        }*/
        /*
        if (OVRInput.GetDown(OVRInput.Button.Start, OVRInput.Controller.Hands))
        {
            Debug.LogWarning("Hand-Tracking Start Button DOWN");
        }

        if (OVRInput.GetUp(OVRInput.Button.Start, OVRInput.Controller.Hands))
        {
            Debug.LogWarning("Hand-Tracking Start Button UP");
        }*/
    }

    public void QuitGame()
    {
        // save any game data here
        #if UNITY_EDITOR
                // Application.Quit() does not work in the editor so
                // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                 Application.Quit();
        #endif
    }
}
