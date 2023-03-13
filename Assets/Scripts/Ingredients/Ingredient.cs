using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Elements { Fire, Dark, Light, Water, Air, Earth, None }
//[RequireComponent(typeof(ParticleSystem))]
public class Ingredient : MonoBehaviour
{
    public enum Ingredients { AngelLeaf, Mandrake, WolfsBane, CorkWood }
    [HideInInspector] public Ingredients ThisIngredient;

    ParticleSystemShapeType ingredientMesh = ParticleSystemShapeType.MeshRenderer;
    public Material FireSpell, AirSpell, WaterSpell, EarthSpell, LightSpell, DarkSpell;

    public int Strength;
    public Elements Element;
    
    protected virtual void Awake()
    {
        //Give random strength
        //strength = Random.Range(0, 3);
        Strength = 4;
        Element = Elements.None;
    }

    private void OnTriggerEnter(Collider collision)
    {
        //If element is different than none do nothing
        if (Element != Elements.None) return;

        switch (collision.gameObject.tag)
        {
            case "Spell/Fire":
                Element = Elements.Fire;
                StartParticles(collision.gameObject.GetComponent<MeshRenderer>().material);
                break;

            case "Spell/Air":
                Element = Elements.Air;
                //StartParticles(AirSpell);
                StartParticles(collision.gameObject.GetComponent<MeshRenderer>().material);
                break;

            case "Spell/Water":
                Element = Elements.Water;
                //StartParticles(WaterSpell);
                StartParticles(collision.gameObject.GetComponent<MeshRenderer>().material);
                break;

            case "Spell/Earth":
                Element = Elements.Earth;
                //StartParticles(EarthSpell);
                StartParticles(collision.gameObject.GetComponent<MeshRenderer>().material);
                break;

            case "Spell/Light":
                Element = Elements.Light;
                //StartParticles(LightSpell);
                StartParticles(collision.gameObject.GetComponent<MeshRenderer>().material);
                break;

            case "Spell/Dark":
                //Darkened= true;
                Element = Elements.Dark;
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
        PSR.material= ElementHit;
        //PS.startColor = color;
        //PSR.material= ElementHit;
        //PS.Play();
        //PS.shape.meshRenderer = gameObject.GetComponent<MeshRenderer>()
    }
}
