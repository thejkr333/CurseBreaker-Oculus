using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reset_position : MonoBehaviour
{
    public CustomerController controller;
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {
        controller.ResetPosition();
    }
}
