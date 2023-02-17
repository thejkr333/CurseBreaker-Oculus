using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yellow : Ingredient
{
    protected override void Awake()
    {
        base.Awake();

        ingredient = Ingredients.Yellow;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
