using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum Ingredients { AngelLeaf, Mandrake, WolfsBane, CorkWood, Nightshade, DragonsTongue }
public class Ingredient : MonoBehaviour
{
    [HideInInspector] public Ingredients ThisIngredient;

    ParticleSystemShapeType ingredientMesh = ParticleSystemShapeType.MeshRenderer;
    public Material FireSpell, AirSpell, WaterSpell, EarthSpell, LightSpell, DarkSpell;

    public int Strength;
    //public Elements Element;
    
    protected virtual void Awake()
    {
        //Give random strength
        //strength = Random.Range(0, 3);
        Strength = 4;
    }
}
