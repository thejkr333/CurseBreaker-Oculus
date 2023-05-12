using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Noise))]
public class CustomerFloat : MonoBehaviour
{
    Noise noise;
    OVRGrabbable grabbable;
    Rigidbody rb;
    public Transform spawn;
    Transform tr;
    [SerializeField] private float speed = .5f, startFloatDistance = .5f, endFloatDistance = 5;
    public bool Attracted;

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
            Attracted = false;
            return;
        }

        if (Attracted)
        {
            noise.enabled = false;
            return;
        }

        rb.useGravity = false;
        tr.eulerAngles = new Vector3(0, spawn.eulerAngles.y, 0);

        float _distanceToTarget = Vector3.Distance(tr.position, spawn.position);

        if (_distanceToTarget < startFloatDistance)
        {
            if (!noise.enabled)
            {
                noise.initialPosition = tr.position;
                noise.enabled = true;
            }
        }
        else
        {
            if (noise.enabled && _distanceToTarget > endFloatDistance)
            {
                noise.enabled = false;
            }

            if (!noise.enabled)
            {
                tr.position = Vector3.MoveTowards(tr.position, spawn.position, speed * Time.deltaTime);
            }
        }
    }
}
