using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//the entire script is here just to avoid accidentally breaking the grabbing incase your hand somehow enters the bin while holding the object
public class Trashable : MonoBehaviour
{
    private OVRGrabbable ovrgrab;
    public bool CanBeDestroyed;
    // Start is called before the first frame update
    void Start()
    {
        ovrgrab = GetComponent<OVRGrabbable>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ovrgrab == null) return;

        CanBeDestroyed = !ovrgrab.isGrabbed;
    }
}
