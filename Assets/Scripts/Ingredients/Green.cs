using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Green : Ingredient
{
    protected override void Awake()
    {
        base.Awake();

        ingredient = Ingredients.Green;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
