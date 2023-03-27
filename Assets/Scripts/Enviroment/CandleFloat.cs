using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CandleFloat : MonoBehaviour
{
    public float LerpSpeed = 1, BaseDistance = 0.1f;

    Vector3 startPos, currentPos; //endPos;

    public bool DoLerp = true;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        //endPos = new Vector3(startPos.x, startPos.y+BaseDistance, startPos.z);
        currentPos = startPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (DoLerp)
        {
            currentPos.y = startPos.y + Mathf.Sin(Time.time * LerpSpeed) * BaseDistance; //Mathf.PingPong(LerpSpeed*Time.time, LerpDistance);//Mathf.SmoothStep(startPos.y, endPos.y, LerpSpeed * Time.time);
            transform.position = currentPos ;
        }
    }
}
