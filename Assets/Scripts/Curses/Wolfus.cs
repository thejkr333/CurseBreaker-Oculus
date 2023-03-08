using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolfus : Curse
{
    protected void Awake()
    {
        curse = Curses.Wolfus;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    protected override void SetUpCurse()
    {
        base.SetUpCurse();

        foreach (var limb in affectedLimbs)
        {
            if (limb.limbName == LimbsList.Soul) continue;
            limb.affectedLimbGO.GetComponentInChildren<MeshRenderer>().material.color = Color.black;
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
