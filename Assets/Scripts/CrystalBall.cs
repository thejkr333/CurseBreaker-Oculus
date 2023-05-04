using UnityEngine;

public class CrystalBall : MonoBehaviour
{
    SphereCollider sphereCollider;
    ParticleSystem particleSystem;

    ParticleSystemRenderer psRenderer;
    [SerializeField] Material red, green;
    private void Awake()
    {
        sphereCollider = GetComponentInChildren<SphereCollider>();
        particleSystem = GetComponentInChildren<ParticleSystem>();
        psRenderer = GetComponentInChildren<ParticleSystemRenderer>();

        sphereCollider.enabled = false;
    }

    private void Start()
    {
        //DayManager.Instance.CustomersFinished += GetReadyForNextDay;     
        DayManager.Instance.customerCured += ChangeParticlesToGreen;
        DayManager.Instance.nextCustomer += StartParticlesRed;
        DayManager.Instance.customerOut += StopParticles;

        StartParticlesRed();
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

        psRenderer.material = red;
    }

    private void ChangeParticlesToGreen()
    {
        psRenderer.material = green;
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
