using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysLookToCam : MonoBehaviour
{
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 dir = cam.transform.position - transform.position;
        //transform.up = dir.normalized;
        transform.LookAt(cam.transform);
    }
}
