using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cursebreaker", menuName = "matrix")]
public class CursexIngredientMatrix : ScriptableObject, ISerializationCallbackReceiver
{
    public Dictionary<Curses, Dictionary<Ingredients, int>> factors;
    List<CurseIngredientFact> factorIngredients = new();

    public void OnAfterDeserialize()
    {
        factors = new();
        foreach (var fact in factorIngredients)
        {
            if(!factors.ContainsKey(fact.curse))
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
}

[System.Serializable]
public class CurseIngredientFact
{
    public Curses curse;
    public Ingredients ingredient;
    public int value;
}



