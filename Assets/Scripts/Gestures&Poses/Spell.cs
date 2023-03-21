using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{ public GameObject FirstHand;
    private float deleteTimer = 10;
    private bool removeSpell;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (deleteTimer < 0 && removeSpell == true)
        {
            Destroy(gameObject);
        }
        if(removeSpell== true)
        {
            deleteTimer-= Time.deltaTime;
        }

        if (FirstHand.GetComponent<PoseGrab>().SpellRelease == true)
        {
            removeSpell = true;
            
        }
        if (FirstHand.GetComponent<PoseGrab>().SpellRelease == false)
            removeSpell = false;
    }
    private void OnTriggerStay(Collider other)
    {
        deleteTimer = 10;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (FirstHand != null && other.gameObject != FirstHand)
        {

           // FirstHand.GetComponent<PoseGrab>().SpellRelease = true;


           //this is here just to stop the errors and prevent them from happening
          
            
            //Errors out if the player is still trying to grab the spell, doing any other pose before it is destroyed prevents it erroring out 
            //Destroy(gameObject);
        }
        if (FirstHand == null)
        {
            if (other.GetComponent<PoseGrab>())
            {
                FirstHand = other.gameObject;
            }
        }
       

       if(other.gameObject == FirstHand)
        {
            removeSpell = false;
        }
        

        //Destroy(gameObject);
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == FirstHand)
        {
           // removeSpell = true;
            deleteTimer = 0.1f;
        }
    }
}
