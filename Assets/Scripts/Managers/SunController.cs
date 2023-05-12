    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class SunController : MonoBehaviour
{
    public int CurrentCustomer;
    public int[] XRotForSun;
    public GameObject Sun;
    public float XRotAnim;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CurrentCustomer = DayManager.Instance.SunState;
        if (CurrentCustomer > 2)
        {
            CurrentCustomer = 2;
        }
        if (CurrentCustomer == 0) Sun.GetComponent<Light>().color = Color.yellow;
        if(CurrentCustomer == 1) Sun.GetComponent<Light>().color = Color.white;
        if( CurrentCustomer == 2) Sun.GetComponent<Light>().color = Color.black;
        if (GameManager.Instance.NextDayAnimation == true)
        {
            Sun.transform.rotation = Quaternion.Euler(XRotAnim, 0, 0);
            
            XRotAnim += Time.deltaTime*10;
            if (XRotAnim >= 400)
            {


                GameManager.Instance.NextDayAnimation = false;
            }
            goto SkipFlicker;
        }
        if (GameManager.Instance.NextDayAnimation == false) XRotAnim = 330;
        Sun.transform.rotation = Quaternion.Euler(XRotForSun[CurrentCustomer], 0, 0);
    SkipFlicker:;
    }
}
