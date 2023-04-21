using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        var lights = FindObjectsOfType<Light>();

        foreach ( Light light in lights )
        {
            if(light.transform.parent.name != "PotV1") light.intensity = 2f;
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
