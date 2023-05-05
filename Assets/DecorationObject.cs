using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorationObject : MonoBehaviour
{
    // Start is called before the first frame update
    Transform parent;
    Vector3 startingPosition;
    Rigidbody rb;
    void Start()
    {
        parent = transform.parent;
        startingPosition = transform.localPosition;
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Floor")
        {
            transform.parent = parent;
            transform.localPosition = startingPosition;
            rb.isKinematic = true;
        }
    }
}
