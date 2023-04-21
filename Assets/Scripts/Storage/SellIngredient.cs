using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellIngredient : MonoBehaviour
{
    public ParticleSystem MoneyParticle;



    private void OnCollisionEnter(Collision collision)
    {
        if (!(FindObjectOfType<GameManager>()))
        {
            Debug.Log("can't find the game manager");
            return;
        }

        if (collision.gameObject.GetComponent<Ingredient>())
        {
            GameManager.Instance.SellIngredient(1);
            
            ParticleSystem temp;
            temp = Instantiate(MoneyParticle, this.transform);
            Destroy(temp.gameObject, 1f);
            Destroy(collision.gameObject);
        }

    }
}
