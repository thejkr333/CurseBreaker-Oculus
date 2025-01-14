using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderController : MonoBehaviour
{

    enum SliderState{OFF, MOVING, FINISHED}
    
    [SerializeField]float successPoint = 0.5f;
    [SerializeField]float successMargin = 0.1f;
    [SerializeField] float speed;
    [SerializeField] GameObject textGO;
    [SerializeField] GameObject background;
    
    
    Slider slider;
    SliderState state = SliderState.OFF;

    float growing = 1;

    Shader shader;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        
        background.GetComponent<Image>().material.SetFloat("_SuccessNumber", successPoint);
        background.GetComponent<Image>().material.SetFloat("_SuccessMargin", successMargin);
    }


    private void OnEnable()
    {
        state = SliderState.OFF;
        background.GetComponent<Image>().material.SetFloat("_SuccessNumber", successPoint);
        background.GetComponent<Image>().material.SetFloat("_SuccessMargin", successMargin);

    }


    private IEnumerator SwitchStateTimer(SliderState newState, float waitingTime)
    {
        yield return new WaitForSeconds(waitingTime);
        state = newState;
        if(state == SliderState.OFF)
        {
            state = SliderState.OFF;
            textGO.SetActive(false);
            slider.value = 0;
        }
       
    }
    // Update is called once per frame
    void Update()
    {
        bool input = Input.GetKeyDown(KeyCode.A);

        switch (state)
        {
            case SliderState.OFF:
                if (input) state = SliderState.MOVING;
                break;
            case SliderState.MOVING:
                slider.value += speed * Time.deltaTime * growing;
                if (slider.value <= 0f || slider.value >= 1f) growing *= -1;

                if (input)
                {
                    state = SliderState.FINISHED;

                    textGO.SetActive(true);
                    bool successBool = true;
                    //activate text saying things and check how good it is
                    if(slider.value >= successPoint - successMargin && slider.value <= successPoint + successMargin)
                    {
                        textGO.GetComponent<TextMeshProUGUI>().text = "SUCCESS";
                        textGO.GetComponent<TextMeshProUGUI>().color = Color.green;
                        successBool = true;
                    }
                    else
                    {
                        textGO.GetComponent<TextMeshProUGUI>().text = "FAIL";
                        textGO.GetComponent<TextMeshProUGUI>().color = Color.red;
                        successBool = false;
                    }

                    //invoke an extern method to tell the caller the result

                    //Create a timer for swithing to FINISHED state automatically
                    StartCoroutine(SwitchStateTimer(SliderState.OFF, 2f));

                }
                break;
            case SliderState.FINISHED:
                if (input)
                {
                    state = SliderState.OFF;
                    textGO.SetActive(false);
                    slider.value = 0;
                }
                break;
        }

        //background.GetComponent<CanvasRenderer>().material.SetFloat("_SuccessNumber", successPoint);
        //background.GetComponent<CanvasRenderer>().material.SetFloat("_SuccessMargin", successMargin);

    }
}
