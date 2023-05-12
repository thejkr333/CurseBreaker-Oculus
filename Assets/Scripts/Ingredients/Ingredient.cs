using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum Ingredients { AngelLeaf, Mandrake, WolfsBane, CorkWood, Nightshade, DragonsTongue }
public class Ingredient : MonoBehaviour
{
    [HideInInspector] public Ingredients ThisIngredient;
    Outline outline;

    public int SellCost = 4;
    public int BuyCost = 4;
    protected virtual void Awake()
    {
        if(TryGetComponent(out outline)) outline.enabled = false;
    }

    protected void Update()
    {
        if(transform.root == null) return;

        if (!transform.root.TryGetComponent(out Outline _parentOutline)) return;
        outline.enabled = _parentOutline.enabled;
    }
}
