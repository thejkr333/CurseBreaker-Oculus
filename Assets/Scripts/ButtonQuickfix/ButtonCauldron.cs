using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCauldron : MonoBehaviour
{
    public Cauldron Cauldron;
    [SerializeField] GameObject sliderMinigameGO;
    SliderController sliderController;  

    // Start is called before the first frame update
    void Start()
    {
        sliderController = sliderMinigameGO.GetComponentInChildren<SliderController>();
    }

    // Update is called once per frame
    void Update()
    {

    }



    private void OnTriggerEnter(Collider other)
    {
        //if(other.GetComponent<PoseGrab>() != null) Cauldron.StirCauldron();
        if (other.GetComponent<PoseGrab>() != null)
        {


            if (!sliderMinigameGO.activeSelf)
            {
                sliderMinigameGO.SetActive(true);
            }
            else
            {
                sliderController.InputReceived();
            }
        }
    }
}
