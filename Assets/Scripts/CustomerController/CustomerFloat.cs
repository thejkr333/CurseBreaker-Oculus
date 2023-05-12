using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Noise))]
public class CustomerFloat : MonoBehaviour
{
    Noise noise;
    OVRGrabbable grabbable;
    Rigidbody rb;
    [HideInInspector] public Transform spawn;
    Transform tr;
    [SerializeField] private float speed = 2, startFloatDistance = .5f, goToPosDistance = 5;
    public bool attracted;

    private void Awake()
    {
        noise = GetComponent<Noise>();
        grabbable = GetComponent<OVRGrabbable>();
        rb = GetComponent<Rigidbody>();

        noise.enabled = false;
        tr = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (grabbable.isGrabbed)
        {
            noise.enabled = false;
            attracted = false;
        }

        if (attracted)
        {
            noise.enabled = false;
            return;
        }

        float _distance = Vector3.Distance(tr.position, spawn.position);
        if (_distance <= startFloatDistance)
        {
            rb.useGravity = false;
            noise.enabled = true;
            noise.initialPosition = spawn.position;
        }
        else 
        {
            noise.enabled = false;
            if(_distance < goToPosDistance)
            {
                rb.useGravity = false;
                Vector3 _dir = spawn.position - tr.position;
                tr.Translate(speed * Time.deltaTime * _dir);
            }
            else rb.useGravity = true;
        }
    }
}
