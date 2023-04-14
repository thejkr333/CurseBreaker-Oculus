using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CustomerController : MonoBehaviour
{
    public GameObject Customer;
    public Transform CustomerSpawn;
    public bool CustomerCured, TestFail;
    private float x, z;

    GameObject currentCustomer;
    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (CustomerCured)
        {
            SuccessfulCure();
            CustomerCured = false;
        }
        if (TestFail)
        {
            FailureCure(); 
            TestFail = false;
        }
        //if(Customer.activeSelf== false)
        //{         
        //    Customer = GameObject.FindGameObjectWithTag("Customer");           
        //}      
    }
    void Spawn()
    {      
        currentCustomer = Instantiate(Customer, CustomerSpawn);
        currentCustomer.name = "NewCustomer";
        currentCustomer.transform.eulerAngles = new Vector3(0, CustomerSpawn.eulerAngles.y, 0);
        currentCustomer.transform.position = new Vector3(CustomerSpawn.position.x, 1.62f, CustomerSpawn.position.z);
    }
    void Despawn()
    {
        Destroy(currentCustomer);
    }
   public void SuccessfulCure()
    {
        Despawn();
        Invoke(nameof(Spawn), 2);
    }
   public void FailureCure()
    {   
        Despawn();
        Invoke(nameof(Spawn), 2);
    }
}
