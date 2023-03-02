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
        
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == inside)
        {
            GameObject clon = Instantiate(ingredientPrefab);
            clon.transform.position = transform.position;
            inside = clon;
        }
    }
}
