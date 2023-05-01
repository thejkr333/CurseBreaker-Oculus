using System.Collections.Generic;
using UnityEngine;

public class Cauldron : MonoBehaviour
{

    private void Start()
    {
        AudioManager.Instance.PlaySoundStatic("fire_crackling", transform.position);
    }

    public List<Ingredients> IngredientsInCauldron = new ();
    //[SerializeField] Recipe[] recipes;

    [SerializeField] GameObject basePotionPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Ingredient _ingredient))
        {
            //if (ingredient.selected) return;

            AddIngredient(_ingredient.ThisIngredient);

            //teleport to parla as destroying it makes it lose the reference in the list IngredientsInCauldron
            _ingredient.transform.position = new Vector3(10000, -10, 10000);
            Destroy(_ingredient.gameObject);
        } 
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Ingredient _ingredient))
        {
            //if (_ingredient.selected) return;

            RemoveIngredient(_ingredient.ThisIngredient);
        }

    }
    void AddIngredient(Ingredients ingredient)
    {
        IngredientsInCauldron.Add(ingredient);

        Debug.Log(ingredient);
    }

    void RemoveIngredient(Ingredients ingredient)
    {
        IngredientsInCauldron.Remove(ingredient);

        Debug.Log(ingredient);
    }

    public void StirCauldron(bool success = true)
    {
        if (IngredientsInCauldron.Count <= 0) return;

        if (success)
        {
            GameObject clon = Instantiate(basePotionPrefab);
            clon.transform.position = transform.GetChild(0).position;

            Potion potion = clon.GetComponent<Potion>();
            if (potion == null) potion = clon.AddComponent<Potion>();
            potion.CreatePotion(IngredientsInCauldron);
        }

        //Reset lists
        IngredientsInCauldron.Clear();
    }

    #region Recipe stuff
    //public void TryCheckIfAnyRecipeCompleted()
    //{
    //    for (int i = 0; i < recipes.Length; i++)
    //    {
    //        for (int j = 0; j < recipes[i].ingredientsRequired.Length; j++)
    //        {
    //            foreach (Ingredient ing in ingredientsInCauldron)
    //            {
    //                if (recipes[i].ingredientsChecked.Contains(ing.ingredient)) continue;

    //                if (ing.ingredient == recipes[i].ingredientsRequired[j])
    //                {
    //                    recipes[i].ingredientsChecked.Add(ing.ingredient);
    //                }

    //                if (recipes[i].ingredientsChecked.Count == recipes[i].ingredientsRequired.Length)
    //                {
    //                    RecipeCompleted(recipes[i]);
    //                    Debug.Log("Recipe number " + i + " completed");
    //                    return;
    //                }
    //            }
    //        }
    //    }
    //}

    //void RecipeCompleted(Recipe recipe)
    //{
    //    GameObject clon = Instantiate(recipe.potionWhenCompleted);
    //    clon.transform.position = transform.GetChild(0).position;

    //    Potion potion = clon.GetComponent<Potion>();
    //    if(potion == null) potion = clon.AddComponent<Potion>();
    //    potion.CreatePotion(ingredientsInCauldron);


    //    //Reset lists
    //    ingredientsInCauldron.Clear();
    //    for (int i = 0; i < recipes.Length ; i++)
    //    {
    //        recipes[i].ingredientsChecked.Clear();
    //    }
    //}
    #endregion
}

//[System.Serializable]
//public class Recipe
//{
//    public Ingredient.Ingredients[] ingredientsRequired;
//    [HideInInspector] public List<Ingredient.Ingredients> ingredientsChecked = new List<Ingredient.Ingredients>();
//    public GameObject potionWhenCompleted;
//}
