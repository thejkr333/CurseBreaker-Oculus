using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiningOrm : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.rotation.y == -359) transform.rotation = Quaternion.Euler(Vector3.zero);
        transform.Rotate(0f, -Time.deltaTime * 4, 0f);

    }
}
