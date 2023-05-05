using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        /*if (!other.TryGetComponent(out OVRGrabbable grabbable)) return;
        
        Destroy(other.gameObject);*/
        if (other.TryGetComponent(out Trashable trashable))
        {
            if(other.GetComponent<Trashable>().trashable == true) Destroy(other.gameObject);
        }
        
    }
}
