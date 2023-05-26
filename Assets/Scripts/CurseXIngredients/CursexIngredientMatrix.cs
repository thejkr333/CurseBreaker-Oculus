using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Cursebreaker", menuName = "matrix")]
public class CursexIngredientMatrix : ScriptableObject, ISerializationCallbackReceiver
{
    public static Dictionary<Curses, Dictionary<Ingredients, int>> factors;
    [SerializeField] List<CurseIngredientFact> factorIngredients = new();

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

    //private void OnEnable()
    //{
    //    EditorUtility.SetDirty(this);
    //}

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
        }
        return _potionStrength;
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

    public static void ReturnIngredientsForCurse(Curses curse, int curseStrength, ref Dictionary<Ingredients, int> ingredients)
    {
        while (curseStrength != 0)
        {
            if (curseStrength >= 5)
            {
                foreach (var item in factors[curse])
                {
                    if(item.Value == 1)
                    {
                        if(ingredients.ContainsKey(item.Key)) ingredients[item.Key]++;
                        else ingredients.Add(item.Key, item.Value);

                        curseStrength -= 5;
                        break;
                    }
                }
            }
            else if(curseStrength > 0) 
            {
                foreach (var item in factors[curse])
                {
                    if (item.Value == 2)
                    {
                        if (ingredients.ContainsKey(item.Key)) ingredients[item.Key]++;
                        else ingredients.Add(item.Key, item.Value);

                        curseStrength -= 2;
                        break;
                    }
                }
            }
            else
            {
                foreach (var item in factors[curse])
                {
                    if (item.Value == 3)
                    {
                        if (ingredients.ContainsKey(item.Key)) ingredients[item.Key]++;
                        else ingredients.Add(item.Key, item.Value);

                        curseStrength += 1;
                        break;
                    }
                }
            }
        }
    }
}

[System.Serializable]
public class CurseIngredientFact
{
    public Curses curse;
    public Ingredients ingredient;
    public int value;
}

public static class Extensions
{
    public static T MaxValueKey<T>(this IDictionary<T, int> dict)
    {
        KeyValuePair<T, int> max = new KeyValuePair<T, int>();
        foreach (var entry in dict)
        {
            if (entry.Value > max.Value)
            {
                max = entry;
            }
        }
        return max.Key;
    }
}



