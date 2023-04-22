using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum Ingredients { AngelLeaf, Mandrake, WolfsBane, CorkWood, Nightshade, DragonsTongue }
public class Ingredient : MonoBehaviour
{
    [HideInInspector] public Ingredients ThisIngredient;

    public int SellCost = 4;
    public int BuyCost = 4;
    protected virtual void Awake()
    {
        if(TryGetComponent(out Outline outline)) outline.enabled = false;
    }
}
