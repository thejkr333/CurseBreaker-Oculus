using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicChat : MonoBehaviour
{
    public float ChatTime;
    public GameObject Hi, Wrong, Bye;
    public bool DespawnOnceDone, Cured, WrongPotion;
    // Start is called before the first frame update
    void Start()
    {
        ChatTime = 5;
    }

    // Update is called once per frame
    void Update()
    {
        ChatTime -= Time.deltaTime;
        if (ChatTime < 0)
        {
            Hi.SetActive(false);
        }
        if (WrongPotion)
        {
            Wrong.SetActive(true);
            if(ChatTime < 0)
            {
                Wrong.SetActive(false);
                WrongPotion = false;
            }
        }
        if(DespawnOnceDone && Cured == true)
        {
            Bye.SetActive(true);
            if (ChatTime < 0)
            {
                GameObject.Find("CustomerController").GetComponent<CustomerController>().CustomerCured = true;
            }
            
        }
        if(DespawnOnceDone && Cured == false)
        {
            Bye.SetActive(true);
            if(ChatTime < 0)
            {
                GameObject.Find("CustomerController").GetComponent<CustomerController>().FailureCure();
            }          
        }
    }
}
