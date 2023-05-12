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
    [SerializeField] private float speed = .5f, startFloatDistance = .5f, goToPosDistance = 5;
    public bool Attracted;
    bool chilling;

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

        float _distance = Vector3.Distance(tr.position, spawn.position);
        if (_distance <= startFloatDistance && !chilling)
        {
            chilling = true;
            noise.enabled = true;
            noise.initialPosition = tr.position;
        }
        else 
        {
            if (chilling)
            {
                if(_distance >= 1)
                {
                    tr.position = Vector3.MoveTowards(tr.position, spawn.position, speed * Time.deltaTime);
                    chilling = false;
                    noise.enabled = false;
                }
            }
            else
            {
                tr.position = Vector3.MoveTowards(tr.position, spawn.position, speed * Time.deltaTime);
            }
        }
    }
}
