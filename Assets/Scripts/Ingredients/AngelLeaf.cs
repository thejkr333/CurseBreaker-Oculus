using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngelLeaf : Ingredient
{
    protected override void Awake()
    {
        base.Awake();

        ingredient = Ingredients.Blue;
    }

    // Update is called once per frame
    void Update()
    {
        
    }



}
