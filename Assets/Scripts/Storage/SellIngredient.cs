using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellIngredient : MonoBehaviour
{
    [SerializeField] ParticleSystem moneyParticle;

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.TryGetComponent(out Ingredient _ingredient)) return;

        GoldManager.Instance.SellIngredient(_ingredient.SellCost);

        ParticleSystem temp = Instantiate(moneyParticle, this.transform);
        Destroy(temp.gameObject, 1f);
        Destroy(collision.gameObject);
    }
}
