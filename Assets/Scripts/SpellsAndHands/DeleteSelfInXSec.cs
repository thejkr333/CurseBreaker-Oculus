using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteSelfInXSec : MonoBehaviour
{
    public float Lifetime = 5f;

    // Start is called before the first frame update
    void Awake()
    {
        Destroy(gameObject, Lifetime);
    }
}
