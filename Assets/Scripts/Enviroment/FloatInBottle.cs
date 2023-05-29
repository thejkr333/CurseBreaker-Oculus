using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatInBottle : MonoBehaviour
{
    public float LerpSpeed = 1, BaseDistance = 0.1f, SpinSpeed = 1f;
    public float MinLerpSpeed = .1f, MaxLerpSpeed = 2, MinBaseDistance = .004f, MaxBaseDistance = .015f, MinSpinSpeed = .25f, MaxSpinSpeed = .5f;

    Vector3 startPos, currentPos; //endPos;

    public bool DoLerp = true, DoSpin = false, InverseSpin = false;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.localPosition;
        //endPos = new Vector3(startPos.x, startPos.y+BaseDistance, startPos.z);
        currentPos = startPos;
        BaseDistance = Random.Range(MinBaseDistance, MaxBaseDistance);
        LerpSpeed = Random.Range(MinLerpSpeed, MaxLerpSpeed);
        SpinSpeed = Random.Range(MinSpinSpeed, MaxSpinSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        if (DoLerp)
        {
            currentPos.y = startPos.y + Mathf.Sin(Time.time * LerpSpeed) * BaseDistance; //Mathf.PingPong(LerpSpeed*Time.time, LerpDistance);//Mathf.SmoothStep(startPos.y, endPos.y, LerpSpeed * Time.time);
            transform.localPosition = currentPos;
        }

        if (DoSpin)
        {
            if (InverseSpin)
            {
                gameObject.transform.Rotate(0f, -SpinSpeed, 0f);
            }else
            {
                gameObject.transform.Rotate(0f, SpinSpeed, 0f);

            }
        }
    }
}
