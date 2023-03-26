using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum Ingredients { AngelLeaf, Mandrake, WolfsBane, CorkWood, Nightshade, DragonsTongue }
public class Ingredient : MonoBehaviour
{
    [HideInInspector] public Ingredients ThisIngredient;

    
    protected virtual void Awake()
    {

    }
}
