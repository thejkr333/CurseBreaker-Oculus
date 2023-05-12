using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfsBane : Ingredient
{
    protected override void Awake()
    {
        base.Awake();

        ThisIngredient = Ingredients.WolfsBane;
    }
}
