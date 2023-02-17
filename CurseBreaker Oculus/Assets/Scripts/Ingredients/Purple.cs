using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Purple : Ingredient
{
    protected override void Awake()
    {
        base.Awake();

        ingredient = Ingredients.Purple;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
