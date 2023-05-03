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

        StartParticlesRed();
    }

    private void Start()
    {
        //DayManager.Instance.CustomersFinished += GetReadyForNextDay;     
        DayManager.Instance.customerCured += ChangeParticlesToGreen;
        DayManager.Instance.nextCustomer += StartParticlesRed;
        DayManager.Instance.customerOut += StopParticles;
    }

    private void StopParticles()
    {
        AudioManager.Instance.StopSound("Crystal_ball");
        particleSystem.Stop();
    }

    private void StartParticlesRed()
    {
        AudioManager.Instance.PlaySoundStatic("Crystal_ball", transform.position);
        particleSystem.Play();
        var main = particleSystem.main;
        main.startColor = Color.red;
    }

    private void ChangeParticlesToGreen()
    {
        var main = particleSystem.main;
        main.startColor = Color.green;
    }

    void GetReadyForNextDay()
    {
        sphereCollider.enabled = true;
        particleSystem.Play();
        AudioManager.Instance.PlaySoundStatic("Crystal_ball", transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        //PoseGrab hand = other.GetComponent<PoseGrab>();
        //if (hand == null) return;
        //sphereCollider.enabled = false;
        //particleSystem.Stop();
        //GameManager.Instance.NextDay();
        //AudioManager.Instance.StopSound("Crystal_ball");
    }
}
