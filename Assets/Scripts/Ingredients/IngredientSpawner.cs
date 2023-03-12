using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSpawner : MonoBehaviour
{
    [SerializeField] GameObject ingredientPrefab;
    [SerializeField] GameObject inside;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(inside.transform.position, transform.position) > 1)
        {
            GameObject clon = Instantiate(ingredientPrefab);
            clon.transform.position = transform.position;
            inside = clon;
        }
    }
}
