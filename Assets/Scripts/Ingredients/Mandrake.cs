using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mandrake : Ingredient
{
    protected override void Awake()
    {
        base.Awake();

        ThisIngredient = Ingredients.Mandrake;
    }

    // Update is called once per frame
    void Update()
    {
        
    }



}
