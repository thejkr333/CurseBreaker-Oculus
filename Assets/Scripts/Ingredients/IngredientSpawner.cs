using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSpawner : MonoBehaviour
{
    [SerializeField] GameObject ingredientPrefab;
    [SerializeField] GameObject inside;
    public bool GotIngredient;
    Vector3 scaleObj;
    private void Awake()
    {
        scaleObj = inside.transform.localScale;

        GameObject clon = Instantiate(ingredientPrefab);
        clon.transform.position = transform.position;
        clon.transform.localScale = scaleObj;
        inside = clon;
    }

    // Update is called once per frame
    void Update()
    {
        //Check if the object iniside is STILL inside, if not create another one
        if(Vector3.Distance(inside.transform.position, transform.position) > 1)
        {
            GameObject clon = Instantiate(ingredientPrefab);
            clon.transform.position = transform.position;
            clon.transform.localScale = scaleObj;
            inside = clon;

            SendMessage("SubtractMoney");
        }
    }
}
