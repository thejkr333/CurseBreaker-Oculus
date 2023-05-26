using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleRandomisation : MonoBehaviour
{

    public ParticleSystem Flames;
    private ParticleSystem.MainModule flamesMain;

    public float minSize = 0.5f, maxSize = 1f, minLife = 0.1f, maxLife = 2f;
    private float minUseSize, maxUseSize;

    public GameObject CandleBody;
    public bool RandomiseBodyColour = true, RandomiseFlameColour = true, UseDefinedColours = true, RandomiseFlameSize = true;
    public Color[] possibleBodyColours;
    public Gradient[] possibleFlameColours;


    // Start is called before the first frame update
    void Awake()
    {
        if (Flames == null)
        {
            Flames = gameObject.GetComponentInChildren<ParticleSystem>();
        }
        flamesMain = Flames.main;

        if (RandomiseFlameSize == true)
            RandomiseFlameSizeVoid();

        if (RandomiseFlameColour == true)
            RandomiseFlameColourVoid();

        //If the body has been defiened and the candle is to use random colours, script will change the colour to either one of predefined or entirely random colours.
        if (CandleBody != null && RandomiseBodyColour )
        {
            if (UseDefinedColours)
            {
                CandleBody.GetComponent<Renderer>().material.color = possibleBodyColours[Random.Range(0, possibleBodyColours.Length)];
            }else
            {
                CandleBody.GetComponent<Renderer>().material.color = Random.ColorHSV(0f,1f,0.1f,1f,0.1f,1f,1f,1f);
            }
        }
    }

    void RandomiseFlameColourVoid()
    {
        var lifeCol = Flames.colorOverLifetime;
        lifeCol.color = possibleFlameColours[Random.Range(0, possibleFlameColours.Length)];
    }

    void RandomiseFlameSizeVoid()
    {
        minUseSize = Random.Range(minSize, maxSize);
        maxUseSize = Random.Range(minSize, maxSize);
        if (minUseSize > maxUseSize)
        {
            (maxUseSize, minUseSize) = (minUseSize, maxUseSize);
        }
        else if (minUseSize == maxUseSize)
        {
            RandomiseFlameSizeVoid();
            return;
        }

        flamesMain.startLifetime = Random.Range(minLife, maxLife);
        flamesMain.startSize = new ParticleSystem.MinMaxCurve(minUseSize, maxUseSize);
    }
}
