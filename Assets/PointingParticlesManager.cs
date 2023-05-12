using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class PointingParticlesManager : MonoBehaviour
{
    public static PointingParticlesManager Instance;

    ParticleSystem particleSystem;
    ParticleSystemForceField destination;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            particleSystem = GetComponent<ParticleSystem>();
            destination = transform.GetComponentInChildren<ParticleSystemForceField>();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    public void NewObjectOutlined(GameObject handAttracting)
    {
        particleSystem.Play();
        transform.parent = null;
        transform.position = handAttracting.transform.position;
        transform.parent = handAttracting.transform.parent;
        transform.LookAt(handAttracting.transform.position);



        destination.transform.parent = null;
        destination.transform.position = handAttracting.transform.position;
        destination.transform.parent = handAttracting.transform;

    }


    public void StopEmitting()
    {
        particleSystem.Stop();
    }

    

    
}
