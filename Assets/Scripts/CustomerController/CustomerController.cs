using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CustomerController : MonoBehaviour
{

    public GameObject Customer, nextCustomer;
    public Transform CustomerSpawn;
    public bool CustomerCured, testFail;
    private float x, z;
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
        if (testFail)
        {
            FailureCure(); 
            testFail = false;
        }
        if(Customer.activeSelf== false)
        {
            
            Customer = GameObject.FindGameObjectWithTag("Customer");
            
            
        }


        
        
    }
    void Spawn()
    {
        Instantiate(Customer, CustomerSpawn);
        Customer = GameObject.FindGameObjectWithTag("Customer");
        Customer.name = "NewCustomer";
        x = CustomerSpawn.position.x;
        z = CustomerSpawn.position.z;
        
    }
    void despawn()
    {
        CustomerSpawn.transform.position = new Vector3(x, 1.62f, z);
        Customer.SetActive(false);
        var copy = Instantiate<GameObject>(nextCustomer);
        
        copy.SetActive(true);
        copy.transform.position = CustomerSpawn.position;
        Customer = copy;
       
    }
   public void SuccessfulCure()
    {
        

        
        despawn();

        
    }
   public void FailureCure()
    {

        
        despawn();
    }
}
