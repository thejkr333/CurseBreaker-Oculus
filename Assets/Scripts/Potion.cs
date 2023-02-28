using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PotionType { }
public class Potion : MonoBehaviour
{
    PotionType potionType;

    List<Elements> ingredientsElements = new();

    int strength;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreatePotion(PotionType type, List<Ingredient> ingredients)
    {
        potionType = type;

        foreach (var ing in ingredients)
        {
            strength += ing.strength;
            ingredientsElements.Add(ing.element);
        }
    }
}
