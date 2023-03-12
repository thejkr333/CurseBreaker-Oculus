using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PotionType { }
public class Potion : MonoBehaviour
{
    public List<Curses> potionTypes = new();

    public List<Elements> ingredientsElements = new();

    public int strength;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreatePotion(List<Ingredient> ingredients)
    {
        foreach (var ing in ingredients)
        {
            switch (ing.ThisIngredient)
            {
                case Ingredient.Ingredients.AngelLeaf:
                    potionTypes.Add(Curses.Demonitis);
                    break;
                case Ingredient.Ingredients.CorkWood:
                    potionTypes.Add(Curses.Gassle);
                    break;
                case Ingredient.Ingredients.Mandrake:
                    potionTypes.Add(Curses.Petrification);
                    break;
                case Ingredient.Ingredients.WolfsBane:
                    potionTypes.Add(Curses.Wolfus);
                    break;
            }

            strength += ing.Strength;
            ingredientsElements.Add(ing.Element);
        }
    }
}
