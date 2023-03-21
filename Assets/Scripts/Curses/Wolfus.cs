using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolfus : Curse
{
    protected void Awake()
    {
        CurrentCurse = Curses.Wolfus;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public override void ChangeVisuals(GameObject affectedLimb)
    {
        base.ChangeVisuals(affectedLimb);

        affectedLimb.GetComponentInChildren<MeshRenderer>().material.color = Color.black;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
