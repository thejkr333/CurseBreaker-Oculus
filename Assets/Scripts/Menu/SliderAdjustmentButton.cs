using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderAdjustmentButton : MonoBehaviour
{
    public Slider Slider;
    public bool Increase, Decrease;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (Increase) IncreaseSlider();
        if(Decrease) DecreaseSlider();
    }
    public void IncreaseSlider()
    {
        Slider.value += Time.deltaTime*2;
    }
    public void DecreaseSlider()
    {
        Slider.value -= Time.deltaTime*2;
    }
}
