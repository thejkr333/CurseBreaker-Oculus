using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Elements { Fire, Dark, Light, Water, Air, Earth, Void }
public class Potion : MonoBehaviour
{
    public List<Ingredients> PotionIngredients = new();

    public List<Elements> PotionElements = new();

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
                StartParticles(collision.gameObject.GetComponent<MeshRenderer>().material);
                break;

            case "Spell/Air":
                PotionElements.Add(Elements.Air);
                //StartParticles(AirSpell);
                StartParticles(collision.gameObject.GetComponent<MeshRenderer>().material);
                break;

            case "Spell/Water":
                PotionElements.Add(Elements.Water);
                //StartParticles(WaterSpell);
                StartParticles(collision.gameObject.GetComponent<MeshRenderer>().material);
                break;

            case "Spell/Earth":
                PotionElements.Add(Elements.Earth);
                //StartParticles(EarthSpell);
                StartParticles(collision.gameObject.GetComponent<MeshRenderer>().material);
                break;

            case "Spell/Light":
                PotionElements.Add(Elements.Light);
                //StartParticles(LightSpell);
                StartParticles(collision.gameObject.GetComponent<MeshRenderer>().material);
                break;

            case "Spell/Dark":
                //Darkened= true;
                PotionElements.Add(Elements.Dark);
                //StartParticles(DarkSpell);
                StartParticles(collision.gameObject.GetComponent<MeshRenderer>().material);
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
}
