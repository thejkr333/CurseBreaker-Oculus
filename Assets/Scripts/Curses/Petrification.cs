using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Petrification : Curse
{
    protected void Awake()
    {
        CurrentCurse = Curses.Petrification;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    protected override void SetUpCurse()
    {
        base.SetUpCurse();

        foreach (var limb in AffectedLimbs)
        {
            if (limb.LimbName == LimbsList.Soul) continue;
            limb.AffectedLimbGO.GetComponentInChildren<MeshRenderer>().material.color = Color.cyan;
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
