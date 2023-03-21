using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Petrification : Curse
{
    protected void Awake()
    {
        CurrentCurse = Curses.Petrification;

        primaryIngredient = Ingredients.Mandrake;
        secondaryIngredient = Ingredients.AngelLeaf;
        substractiveIngredient = Ingredients.Nightshade;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public override void ChangeVisuals(GameObject affectedLimb)
    {
        base.ChangeVisuals(affectedLimb);

        affectedLimb.GetComponentInChildren<MeshRenderer>().material.color = Color.cyan;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
