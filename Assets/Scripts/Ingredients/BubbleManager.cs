using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BubbleManager : MonoBehaviour
{
    Transform[] wayPoints;
    GameObject[] bubbles;

    [SerializeField] int maxNumBubbles;

    [SerializeField] GameObject bubblePrefab;
    [SerializeField] GameObject[] ingredientsPrefab;
    Dictionary<Ingredients, GameObject> ingredients = new();

    private void Awake()
    {
        PopulateWayPoints();

        CreateBubbles();
    }

    void PopulateWayPoints()
    {
        wayPoints = new Transform[transform.childCount];
        for (int i = 0; i < wayPoints.Length; i++)
        {
            wayPoints[i] = transform.GetChild(i);
        }
    }

    void CreateBubbles()
    {
        bubbles = new GameObject[maxNumBubbles];
        for (int i = 0; i < maxNumBubbles; i++)
        {
            //Create bubble
            bubbles[i] = Instantiate(bubblePrefab);

            //Create ingredient inside bubble
            GameObject _clon = Instantiate(ingredientsPrefab[i], bubbles[i].transform.GetChild(0));

            //Save which ingredient is inside the bubble
            Ingredients _ing = _clon.GetComponent<Ingredient>().ThisIngredient;
            bubbles[i].GetComponent<Bubble>().ingredientInside = _ing;
            ingredients.Add(_ing, ingredientsPrefab[i]);
        }
    }
}
