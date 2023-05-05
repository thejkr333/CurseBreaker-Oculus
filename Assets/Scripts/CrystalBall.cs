using System.Collections;
using UnityEngine;

public class CrystalBall : MonoBehaviour
{
    SphereCollider sphereCollider;
    ParticleSystem ps;

    ParticleSystemRenderer psRenderer;
    //[SerializeField] Material particlesRed, particlesGreen;
    //[SerializeField] Material sphereRed, sphereGreen;
    [SerializeField] Color sphereRed, sphereGreen;

    [SerializeField] GameObject particles;
    [SerializeField] MeshRenderer sphere;
    float duration = 2.0f;
    private void Awake()
    {
        sphereCollider = GetComponentInChildren<SphereCollider>();
        ps = GetComponentInChildren<ParticleSystem>();
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
        //ps.Stop();

        for (int i = 0; i < particles.transform.childCount; i++)
        {
            particles.transform.GetChild(i).GetComponent<ParticleSystem>().Stop();
        }
    }

    private void StartParticlesRed()
    {
        AudioManager.Instance.PlaySoundStatic("Crystal_ball", transform.position);
        //ps.Play();

        //psRenderer.material = particlesRed;
        for (int i = 0; i < particles.transform.childCount; i++)
        {
            var main = particles.transform.GetChild(i).GetComponent<ParticleSystem>().main;
            main.startColor = sphereRed;
            particles.transform.GetChild(i).GetComponent<ParticleSystem>().Play();
        }

        StartCoroutine(ChangeMaterialColor(sphereRed));
    }

    private void ChangeParticlesToGreen()
    {
        //psRenderer.material = particlesGreen;
        for (int i = 0; i < particles.transform.childCount; i++)
        {
            var main = particles.transform.GetChild(i).GetComponent<ParticleSystem>().main;
            main.startColor = sphereGreen;
        }

        StartCoroutine(ChangeMaterialColor(sphereGreen));
    }

    IEnumerator ChangeMaterialColor(Color endColor)
    {
        Color _sphereMatCol = sphere.material.color;
        float _timer = 0;
        while (_timer < duration)
        {
            _timer += Time.deltaTime;
            sphere.material.color = Color.Lerp(_sphereMatCol, endColor, _timer/duration);
            yield return null;
        }
        sphere.material.color = endColor;
    }

    void GetReadyForNextDay()
    {
        sphereCollider.enabled = true;
        ps.Play();
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
