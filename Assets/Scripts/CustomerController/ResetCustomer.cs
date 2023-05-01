using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetCustomer : MonoBehaviour
{
    public CustomerController controller;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PoseGrab>() != null) DayManager.Instance.ResetCustomerPos();
    }
}
