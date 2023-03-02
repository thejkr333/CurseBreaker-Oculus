using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSingleton : MonoBehaviour
{
    public CustomerController controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("CustomerTest").GetComponent<CustomerController>();
        if (controller.Customer != null && controller.Customer != this)
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
