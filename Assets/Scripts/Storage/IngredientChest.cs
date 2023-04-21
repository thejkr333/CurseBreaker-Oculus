using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IngredientSpawner))] 
public class IngredientChest : MonoBehaviour
{
    public Ingredients ingredeint;
    string chestName;
    public int cost;
    public ParticleSystem MoneyParticle;

    private void Start()
    {
        chestName = ingredeint.ToString();
    }
    public void SubtractMoney(GameObject ingredient)
    {
        GameManager.Instance.GoldSubtract(cost);
        MoneyParticle.Play();
    }


}
