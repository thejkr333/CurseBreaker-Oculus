using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Elements { Fire, Dark, Light, Water, Air, Earth, Void }
public class Potion : MonoBehaviour
{
    public List<Ingredients> PotionIngredients = new();

    public List<Elements> PotionElements = new();

    [SerializeField] GameObject elementParticlesPrefab;

    public int Strength;
    public void CreatePotion(List<Ingredients> ingredients)
    {
        PotionIngredients = new List<Ingredients>(ingredients);
    }

    private void OnTriggerEnter(Collider collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Spell/Fire":
                PotionElements.Add(Elements.Fire);
                AddParticles(Color.red);
                break;

            case "Spell/Air":
                PotionElements.Add(Elements.Air);
                AddParticles(Color.white);
                break;

            case "Spell/Water":
                PotionElements.Add(Elements.Water);
                AddParticles(Color.blue);
                break;

            case "Spell/Earth":
                PotionElements.Add(Elements.Earth);
                AddParticles(Color.green);
                break;

            case "Spell/Light":
                PotionElements.Add(Elements.Light);
                AddParticles(Color.yellow);
                break;

            case "Spell/Dark":
                //Darkened= true;
                PotionElements.Add(Elements.Dark);
                AddParticles(Color.black);
                break;

            default:
                Debug.Log("Unknown");
                break;
        }
    }

    void StartParticles(Material ElementHit)
    {
        ParticleSystem PS = gameObject.AddComponent<ParticleSystem>();
        ParticleSystemRenderer PSR = gameObject.GetComponent<ParticleSystemRenderer>();
        PSR.material = ElementHit;
        //PS.startColor = color;
        //PSR.material= ElementHit;
        //PS.Play();
        //PS.shape.meshRenderer = gameObject.GetComponent<MeshRenderer>()
    }

    void AddParticles(Color color)
    {
        GameObject clon = Instantiate(elementParticlesPrefab, this.transform);
        for (int i = 0; i < clon.transform.childCount; i++)
        {
            var main = clon.transform.GetChild(i).GetComponent<ParticleSystem>().main;
            main.startColor = color;
        }
    }
}
