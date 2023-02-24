using System.Collections.Generic;
using UnityEngine;

public class Curse : MonoBehaviour
{
    public class AffectedPart
    {
        public GameObject affectedPartGO;
        public int strength;
        public bool cured;

        public AffectedPart(GameObject _affectedPartGO)
        {
            affectedPartGO = _affectedPartGO;
            strength = Random.Range(1, 5);
            cured = false;
        }
    }

    [Range(0, 6)]
    [SerializeField] protected int numberOfPartsAffected;

    protected enum Curses { Wolfus, Gassle, Demonitis, Petrification }
    protected Curses curse;

    protected List<GameObject> possibleAffectedParts = new List<GameObject>();

    protected List<AffectedPart> affectedParts = new List<AffectedPart>();
    protected int totalStrength;

    protected virtual void Awake()
    {
        for (int i = 0; i < numberOfPartsAffected; i++)
        {
            int part = Random.Range(0, possibleAffectedParts.Count);
            affectedParts.Add(new AffectedPart(possibleAffectedParts[part]));
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        totalStrength = 0;
        foreach (var part in affectedParts)
        {
            totalStrength += part.strength;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
