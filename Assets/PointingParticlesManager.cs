using Oculus.Platform.Models;
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
            //Debug.Log("AAAAAAA" + Vector3.Distance(destination.transform.position, particleSystem.transform.position));
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    public void NewObjectOutlined(GameObject handAttracting, GameObject objectAttracted)
    {
        destination.transform.parent = handAttracting.transform;
        destination.transform.localPosition = Vector3.zero;


        transform.SetParent(objectAttracted.transform, true);
        transform.localPosition = Vector3.zero;

        transform.LookAt(handAttracting.transform.position);

        var main = particleSystem.main;
        main.startLifetime = 3 * Vector3.Distance(destination.transform.position, particleSystem.transform.position);
        particleSystem.Play();
    }


    

    public void StopEmitting()
    {
        particleSystem.Stop();
        transform.parent = null;
    }

    

    
}
