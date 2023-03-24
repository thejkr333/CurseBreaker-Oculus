using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{ public GameObject FirstHand;
   // private float deleteTimer = 10;
    private bool removeSpell;
    public bool testDebug;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (/*deleteTimer < 0 &&*/ removeSpell == true&&testDebug == false)
        {
            Destroy(gameObject);
        }
        if(removeSpell== true)
        {
           /* deleteTimer-= Time.deltaTime; */
        }

        transform.position = FirstHand.transform.position;

    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (FirstHand != null && other.gameObject != FirstHand)
        {

            // it now checks if it cant find the grabbable script, so effectively an ingredient or other object, then if it doesnt see that it destroys itself, meaning at most it should apply the effect to the ingredient before destroying itself

            if(other.gameObject.GetComponent<OVRGrabbable>() == null)
            { //this is here just to stop the errors and prevent them from happening


                //Errors out if the player is still trying to grab the spell, doing any other pose before it is destroyed prevents it erroring out 

                //Reenabling this bit and testing how it might work if you cant grab it and it follows the hand directly, no grab needed
                Destroy(gameObject);
            }
           // FirstHand.GetComponent<PoseGrab>().SpellRelease = true;


          
           
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
        /*if(other.gameObject == FirstHand)
        {
           // removeSpell = true;
            deleteTimer = 0.1f;
        }
        if (FirstHand.GetComponent<PoseGrab>().SpellRelease == true)
        {
            removeSpell = true;

        }
        if (FirstHand.GetComponent<PoseGrab>().SpellRelease == false)
        {
            removeSpell = false;
        }*/
    }
}
