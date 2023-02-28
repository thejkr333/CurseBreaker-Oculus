using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cauldron : MonoBehaviour
{
    List<Ingredient> ingredientsInCauldron = new List<Ingredient>();
    [SerializeField] Recipe[] recipes;
    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other)
    {
        Ingredient ingredient = other.GetComponent<Ingredient>();

        if (ingredient == null) return;
        //if (ingredient.selected) return;

        AddIngredient(ingredient);
        //Destroy(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        Ingredient ingredient = other.GetComponent<Ingredient>();

        if (ingredient == null) return;
        //if (ingredient.selected) return;

        RemoveIngredient(ingredient);
    }
    void AddIngredient(Ingredient ingredient)
    {
        ingredientsInCauldron.Add(ingredient);

        Debug.Log(ingredient);
    }

    void RemoveIngredient(Ingredient ingredient)
    {
        ingredientsInCauldron.Remove(ingredient);

        Debug.Log(ingredient);
    }

    public void TryCheckIfAnyRecipeCompleted()
    {
        for (int i = 0; i < recipes.Length; i++)
        {
            for (int j = 0; j < recipes[i].ingredientsRequired.Length; j++)
            {
                foreach (Ingredient ing in ingredientsInCauldron)
                {
                    if (recipes[i].ingredientsChecked.Contains(ing.ingredient)) continue;

                    if (ing.ingredient == recipes[i].ingredientsRequired[j])
                    {
                        recipes[i].ingredientsChecked.Add(ing.ingredient);
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

        Potion potion = clon.GetComponent<Potion>();
        if(potion == null) potion = clon.AddComponent<Potion>();
        potion.CreatePotion(recipe.potionType, ingredientsInCauldron);
        

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
    public Curses potionType;
}
