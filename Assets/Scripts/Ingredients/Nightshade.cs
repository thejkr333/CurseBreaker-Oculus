using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nightshade : Ingredient
{
    protected override void Awake()
    {
        base.Awake();

        ThisIngredient = Ingredients.Nightshade;
    }
}
