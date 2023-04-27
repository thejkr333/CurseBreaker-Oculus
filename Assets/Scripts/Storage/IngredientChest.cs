using UnityEngine;

[RequireComponent (typeof(Outline))]
[RequireComponent(typeof(Rigidbody))]
public class IngredientChest : MonoBehaviour
{
    //[SerializeField] Ingredients ingredient;
    //[SerializeField] int cost;
    [SerializeField] ParticleSystem moneyParticle;
    [SerializeField] GameObject ingredientPrefab;

    private void Awake()
    {
        GetComponent<Outline>().enabled = false;
    }

    void SubtractMoney()
    {   
        GoldManager.Instance.SubstractGold(ingredientPrefab.GetComponent<Ingredient>().BuyCost, TransactionType.Ingredient);
        AudioManager.Instance.PlaySoundStatic("buy", transform.position);

        PlayParticleEffect();
    }

    void PlayParticleEffect()
    {
        ParticleSystem temp = Instantiate(moneyParticle,this.transform);
        Destroy(temp.gameObject, 1f);
    }

    public GameObject InstantiateIngredient()
    {
        if (GoldManager.Instance.Gold < ingredientPrefab.GetComponent<Ingredient>().BuyCost) return null;

        GameObject clon = Instantiate(ingredientPrefab);
        clon.transform.position = transform.position;
        SubtractMoney();
        return clon;
    }
}
