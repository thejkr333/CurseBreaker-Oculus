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
    public void SubtractMoney()
    {   
        if (!(FindObjectOfType<GameManager>())) return;

        GameManager.Instance.GoldSubtract(cost);
        
        PlayParticleEffect();

    }

    void PlayParticleEffect()
    {
        ParticleSystem temp;
        temp = Instantiate(MoneyParticle,this.transform);
        Destroy(temp.gameObject, 1f);
    }

}
