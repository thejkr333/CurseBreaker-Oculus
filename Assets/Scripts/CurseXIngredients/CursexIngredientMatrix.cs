using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Cursebreaker", menuName = "matrix")]
public class CursexIngredientMatrix : ScriptableObject, ISerializationCallbackReceiver
{
    static Dictionary<Curses, Dictionary<Ingredients, int>> factors;
    List<CurseIngredientFact> factorIngredients = new();

    public void OnAfterDeserialize()
    {
        factors = new();
        foreach (var fact in factorIngredients)
        {
            if (!factors.ContainsKey(fact.curse))
            {
                factors.Add(fact.curse, new Dictionary<Ingredients, int>());
            }

            if (fact.value < 0 || fact.value > 3) fact.value = 0;
            factors[fact.curse].Add(fact.ingredient, fact.value);
        }
    }

    public void OnBeforeSerialize()
    {
        factorIngredients.Clear();
        foreach (var item in factors)
        {
            foreach (var value in item.Value)
            {
                factorIngredients.Add(new CurseIngredientFact { curse = item.Key, ingredient = value.Key, value = value.Value });
            }
        }
    }

    public int GetValue(Curses curse, Ingredients ingredient)
    {
        if (!factors.ContainsKey(curse)) return 0;
        if (!factors[curse].ContainsKey(ingredient)) return 0;

        return factors[curse][ingredient];
    }
    public void SetValue(Curses curse, Ingredients ingredient, int value)
    {
        if (!factors.ContainsKey(curse)) factors.Add(curse, new Dictionary<Ingredients, int>());

        if (value < 0 || value > 3) value = 0;

        if (!factors[curse].ContainsKey(ingredient)) factors[curse].Add(ingredient, value);
        else factors[curse][ingredient] = value;
    }

    public static int CalculatePotionStrenght(Curses curse, List<Ingredients> ingredients)
    {
        int _potionStrength = 0;
        foreach (Ingredients ing in ingredients)
        {
            switch (factors[curse][ing])
            {
                case 1:
                    _potionStrength += 5;
                    break;
                case 2:
                    _potionStrength += 2;
                    break;
                case 3:
                    _potionStrength -= 1;
                    break;
            }
            //If an ingredient is not needed give the main curse of that ingredient
        }
        return 0;
    }

    public static Curses GetRandomCurse(List<Ingredients> ingredients)
    {
        int _random = Random.Range(0, ingredients.Count);
        Curses _curse = Curses.Wolfus;

        foreach (var fact in factors)
        {
            if (fact.Value[ingredients[_random]] == 1)
            {
                _curse = fact.Key;
                break;
            }
        }

        return _curse;
    }
}

[System.Serializable]
public class CurseIngredientFact
{
    public Curses curse;
    public Ingredients ingredient;
    public int value;
}



