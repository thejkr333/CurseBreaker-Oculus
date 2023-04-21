using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalBall : MonoBehaviour
{
    SphereCollider sphereCollider;
    ParticleSystem particleSystem;
    private void Awake()
    {
        sphereCollider = GetComponentInChildren<SphereCollider>();
        particleSystem = GetComponentInChildren<ParticleSystem>();

        sphereCollider.enabled = false;

        DayManager.Instance.CustomersFinished += GetReadyForNextDay;
    }

    void GetReadyForNextDay()
    {
        sphereCollider.enabled = true;
        particleSystem.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        PoseGrab hand = other.GetComponent<PoseGrab>();
        if (hand == null) return;
        sphereCollider.enabled = false;
        particleSystem.Stop();
        GameManager.Instance.NextDay();
    }
}
