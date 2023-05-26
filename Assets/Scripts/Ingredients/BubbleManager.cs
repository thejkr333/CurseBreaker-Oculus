using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleManager : MonoBehaviour
{
    Transform[] wayPoints;
    GameObject[] bubbles;

    int maxNumBubbles;
    [SerializeField] float maxMoveDistance;
    [SerializeField] Vector2 minMaxRotSpeed;

    [SerializeField] GameObject bubblePrefab;
    [SerializeField] GameObject[] ingredientsPrefab;
    Dictionary<Ingredients, GameObject> ingredientsBubble = new();

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
        maxNumBubbles = Enum.GetValues(typeof(Ingredients)).Length;
        bubbles = new GameObject[maxNumBubbles];
        for (int i = 0; i < maxNumBubbles; i++)
        {
            //Create bubble
            bubbles[i] = Instantiate(bubblePrefab);

            //Set bubble and dictionary
            Ingredients _ing = SetBubble(ref bubbles[i], ingredientsPrefab[i]);
            ingredientsBubble.Add(_ing, ingredientsPrefab[i]);         
        }
    }
    Ingredients SetBubble(ref GameObject bubble, GameObject ingredientInside)
    {
        //Set bubble pos
        bubble.transform.position = wayPoints[UnityEngine.Random.Range(0, wayPoints.Length)].position;

        //Create ingredient inside and set it
        GameObject _clon = Instantiate(ingredientInside, bubble.transform);
        _clon.layer = 0;
        _clon.GetComponent<Rigidbody>().isKinematic = true;
        _clon.transform.localPosition = Vector3.zero;
        _clon.transform.rotation = Quaternion.identity;
        _clon.transform.localScale = Vector3.one * .8f;


        Ingredient _ing = _clon.GetComponent<Ingredient>();
        Bubble _bubble = bubble.GetComponent<Bubble>();

        _bubble.IngredientInside = _ing;
        _bubble.WayPoints = wayPoints;
        _bubble.MaxMoveDistance = maxMoveDistance;
        _bubble.RotSpeed = UnityEngine.Random.Range(minMaxRotSpeed.x, minMaxRotSpeed.y);
        _bubble.BubbleManager = this;

        return _ing.ThisIngredient;
    }

    public void PopBubble(Ingredients ingredient)
    {
        if(ingredientsBubble.TryGetValue(ingredient, out GameObject ingredientGO))
        {
            for (int i = 0; i < maxNumBubbles; i++)
            {
                if (bubbles[i].GetComponent<Bubble>().IngredientInside.ThisIngredient == ingredient)
                {
                    AudioManager.Instance.PlaySoundStatic("Pop", bubbles[i].transform.position);
                    PointingParticlesManager.Instance?.StopEmitting();
                    if (bubbles[i].transform.childCount == 0)
                    {
                        Destroy(bubbles[i]);
                        bubbles[i] = Instantiate(bubblePrefab);
                        SetBubble(ref bubbles[i], ingredientGO);
                    }
                    else
                    {
                        StartCoroutine(Co_DissolveBubble(bubbles[i], i, ingredientGO));
                    }
                    break;
                }
            }
        }
    }

    IEnumerator Co_DissolveBubble(GameObject bubble, int bubbleNumber, GameObject ingredientGO)
    {
        MeshRenderer _bubbleRenderer = bubble.GetComponent<MeshRenderer>();
        float _dissolveAmount = _bubbleRenderer.material.GetFloat("_DissolveAmount");

        Rigidbody _ingRb = bubble.transform.GetChild(0).GetComponent<Rigidbody>();
        _ingRb.isKinematic = false;
        _ingRb.useGravity = true;
        _ingRb.transform.parent = null;
        while (_dissolveAmount < 1)
        {
            _dissolveAmount += Time.deltaTime;
            _bubbleRenderer.material.SetFloat("_DissolveAmount", _dissolveAmount);
            yield return null;
        }

         Destroy(bubble);
         bubbles[bubbleNumber] = Instantiate(bubblePrefab);
         SetBubble(ref bubbles[bubbleNumber], ingredientGO);
    }
}
