using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demonitis : Curse
{
    protected void Awake()
    {
        curse = Curses.Demonitis;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    protected override void SetUpCurse()
    {
        foreach (var limb in affectedLimbs)
        {
            if (limb.limbName == LimbsList.Soul) continue;
            limb.affectedLimbGO.GetComponentInChildren<MeshRenderer>().material.color = Color.magenta;
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
