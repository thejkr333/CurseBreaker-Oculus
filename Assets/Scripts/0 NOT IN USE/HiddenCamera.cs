using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenCamera : MonoBehaviour
{
    [SerializeField] Transform portal;

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(portal);
    }
}
