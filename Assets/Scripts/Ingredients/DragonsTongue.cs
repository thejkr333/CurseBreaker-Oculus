using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonsTongue : Ingredient
{
    protected override void Awake()
    {
        base.Awake();

        ThisIngredient = Ingredients.DragonsTongue;
    }
}
