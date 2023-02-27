using System.Collections.Generic;
using UnityEngine;

public class Curse : MonoBehaviour
{
    public class AffectedLimb
    {
        public GameObject affectedLimbGO;
        public int strength;
        public bool cured;

        public AffectedLimb(GameObject _affectedLimbGO)
        {
            affectedLimbGO = _affectedLimbGO;
            strength = Random.Range(1, 5);
            cured = false;
        }
    }

    [Range(0, 6)]
    [SerializeField] protected int numberOfPartsAffected;

    protected enum Curses { Wolfus, Gassle, Demonitis, Petrification }
    protected Curses curse;

    protected List<GameObject> possibleAffectedLimbs = new List<GameObject>();

    protected List<AffectedLimb> affectedLimbs = new List<AffectedLimb>();
    protected int totalStrength;
    protected bool cured;

    protected virtual void Awake()
    {
        for (int i = 0; i < numberOfPartsAffected; i++)
        {
            int part = Random.Range(0, possibleAffectedLimbs.Count);
            affectedLimbs.Add(new AffectedLimb(possibleAffectedLimbs[part]));
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        totalStrength = 0;
        foreach (var part in affectedLimbs)
        {
            totalStrength += part.strength;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (cured) return;
        foreach (var limb in affectedLimbs)
        {
            if (!limb.cured) return;
        }
        Cured();
    }

    //void GetPotion(Potion potion)
    //{
    //    foreach (var limb in affectedLimbs)
    //    {
    //        if(potion.limb == limb)
    //        {
    //            if (potion.strength >= limb.strength) limb.cured = true;
    //        }
    //    }
    //}

    void Cured()
    {
        cured = true;
        GameManager.Instance.GoldGain();
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    Potion potion = other.GetComponent<Potion>();
    //    if (potion == null) return;

    //    GetPotion(potion);
    //}
}
