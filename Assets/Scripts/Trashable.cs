using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//the entire script is here just to avoid accidentally breaking the grabbing incase your hand somehow enters the bin while holding the object
public class Trashable : MonoBehaviour
{
    private BoxCollider collider;
    private OVRGrabbable ovrgrab;
    public bool trashable;
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<BoxCollider>();
        ovrgrab = GetComponent<OVRGrabbable>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ovrgrab.isGrabbed != true) trashable = true;
        else trashable = false;
    }
}
