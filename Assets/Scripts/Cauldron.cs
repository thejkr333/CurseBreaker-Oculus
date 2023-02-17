using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cauldron : MonoBehaviour
{
    List<Ingredient.Ingredients> ingredientsInCauldron = new List<Ingredient.Ingredients>();
    [SerializeField] Recipe[] recipes;
    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other)
    {
        Ingredient ingredient = other.GetComponent<Ingredient>();

        if (ingredient == null) return;
        if (ingredient.selected) return;

        AddIngredient(ingredient.ingredient);
        Destroy(other.gameObject);
    }
    void AddIngredient(Ingredient.Ingredients ingredient)
    {
        ingredientsInCauldron.Add(ingredient);

        Debug.Log(ingredient);

        TryCheckIfAnyRecipeCompleted();
    }

    void TryCheckIfAnyRecipeCompleted()
    {
        for (int i = 0; i < recipes.Length; i++)
        {
            for (int j = 0; j < recipes[i].ingredientsRequired.Length; j++)
            {
                foreach (Ingredient.Ingredients ing in ingredientsInCauldron)
                {
                    if (recipes[i].ingredientsChecked.Contains(ing)) continue;

                    if (ing == recipes[i].ingredientsRequired[j])
                    {
                        recipes[i].ingredientsChecked.Add(ing);
                    }

                    if (recipes[i].ingredientsChecked.Count == recipes[i].ingredientsRequired.Length)
                    {
                        RecipeCompleted(recipes[i]);
                        Debug.Log("Recipe number " + i + " completed");
                        return;
                    }
                }
            }
        }
    }

    void RecipeCompleted(Recipe recipe)
    {
        GameObject clon = Instantiate(recipe.potionWhenCompleted);
        clon.transform.position = transform.GetChild(0).position;

        //Reset lists
        ingredientsInCauldron.Clear();
        for (int i = 0; i < recipes.Length ; i++)
        {
            recipes[i].ingredientsChecked.Clear();
        }
    }
}

[System.Serializable]
public class Recipe
{
    //[SerializeField] Ingredient[] ingredientsRequired;
    public Ingredient.Ingredients[] ingredientsRequired;
    [HideInInspector] public List<Ingredient.Ingredients> ingredientsChecked = new List<Ingredient.Ingredients>();
    public GameObject potionWhenCompleted;
}
