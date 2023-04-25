using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class SunController : MonoBehaviour
{
    public int CurrentCustomer;
    public int[] XRotForSun;
    public GameObject Sun;
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

        Sun.transform.rotation = Quaternion.Euler(XRotForSun[CurrentCustomer], 0, 0);
    }
}
