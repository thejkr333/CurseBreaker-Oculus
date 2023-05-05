using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorationObject : MonoBehaviour
{
    // Start is called before the first frame update
    Transform parent;
    Vector3 startingPosition;
    Rigidbody rb;
    Quaternion initialRotation;
    void Start()
    {
        parent = transform.parent;
        startingPosition = transform.localPosition;
        rb = GetComponent<Rigidbody>();
        initialRotation = transform.rotation;
    }


    public void StartGrabbing()
    {
        transform.parent = null;
        rb.isKinematic = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Floor")
        {
            transform.rotation = initialRotation;
            transform.parent = parent;
            transform.localPosition = startingPosition;
            rb.isKinematic = true;
        }
    }
}
