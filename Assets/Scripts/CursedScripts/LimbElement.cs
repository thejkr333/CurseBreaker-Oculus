using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimbElement : MonoBehaviour
{
    public bool Light, Dark, Fire, Ice, Wind, Water, Void;

    public element limb_element;

    public bool isCursed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //public void potion_recived(element, )
    //{

    //}
    

}

public enum element
{
    Light, Dark, Fire, Ice, Wind, Water, Void
}
