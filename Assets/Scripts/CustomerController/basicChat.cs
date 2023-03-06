using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicChat : MonoBehaviour
{
   public float chatTime;
    public GameObject Hi, Wrong, Bye;
    public bool DespawnOnceDone, Cured, wrongPotion;
    // Start is called before the first frame update
    void Start()
    {
        chatTime = 5;
    }

    // Update is called once per frame
    void Update()
    {
        chatTime -= Time.deltaTime;
        if (chatTime < 0)
        {
            Hi.SetActive(false);
        }
        if (wrongPotion)
        {
            Wrong.SetActive(true);
            if(chatTime < 0)
            {
                Wrong.SetActive(false);
                wrongPotion = false;
            }
        }
        if(DespawnOnceDone && Cured == true)
        {
            Bye.SetActive(true);
            if (chatTime < 0)
            {
                GameObject.Find("CustomerController").GetComponent<CustomerController>().CustomerCured = true;
            }
            
        }
        if(DespawnOnceDone && Cured == false)
        {
            Bye.SetActive(true);
            if(chatTime < 0)
            {
                GameObject.Find("CustomerController").GetComponent<CustomerController>().FailureCure();
            }
            
        }
    }
}
