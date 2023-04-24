using UnityEngine;

public class Bubbles : MonoBehaviour
{
    ParticleSystem bubbleSystem;
    ParticleSystem.Particle[] bubbles;
    int prevNumBubbles = 0;

    float bubbleTimer, nextPopTime;
    [SerializeField] Vector2 popMinMaxTime;

    // Start is called before the first frame update
    void Start()
    {
        bubbleSystem = GetComponent<ParticleSystem>();
        bubbles = new ParticleSystem.Particle[bubbleSystem.main.maxParticles];

        nextPopTime = Random.Range(popMinMaxTime.x, popMinMaxTime.y);
        bubbleTimer = nextPopTime;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!bubbleSystem.isPlaying) return;

        PopBubblesTimer();
    }

    void PopBubblesTimer()
    {
        bubbleTimer -= Time.deltaTime;
        if (bubbleTimer <= 0)
        {
            PopBubble();
            nextPopTime = Random.Range(popMinMaxTime.x, popMinMaxTime.y);
            bubbleTimer = nextPopTime;
        }
    }

    void PopBubbleArray()
    {
        int _bubblesAlive = bubbleSystem.GetParticles(bubbles);

        if (_bubblesAlive < prevNumBubbles)
        {
            PopBubble();
            //int _poppedBubbles = prevNumBubbles - _bubblesAlive;
            //for (int i = 0; i < _poppedBubbles; i++)
            //{
            //    PopBubble();
            //    //if (i == 0) PopBubble();
            //    //else
            //    //{
            //    //    float _waitTime = Random.Range(0.1f * i, 0.3f * i);
            //    //    Invoke(nameof(PopBubble), _waitTime);
            //    //}
            //}
        }

        prevNumBubbles = _bubblesAlive;
    }

    private void PopBubble()
    {
        float _pitch = Random.Range(.5f, 5f);
        AudioManager.Instance.PlaySoundStatic("Pop", transform.position, _pitch);
    }
}
