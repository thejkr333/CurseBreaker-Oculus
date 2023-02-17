using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Red : Ingredient
{
    protected override void Awake()
    {
        base.Awake();

        ingredient = Ingredients.Red;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
