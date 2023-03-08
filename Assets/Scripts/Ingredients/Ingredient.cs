using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Elements { Fire, Dark, Light, Water, Air, Earth, None }
//[RequireComponent(typeof(ParticleSystem))]
public class Ingredient : MonoBehaviour
{
    //public bool Burned, Gassed, Drenched, Dusted, Shining, Darkened;
    public enum Ingredients { AngelLeaf, Mandrake, WolfsBane, CorkWood }
    [HideInInspector] public Ingredients ingredient;

    //bool hasBeenSelected;

    [HideInInspector] public bool selected;

    //ParticleSystem PS;
    //ParticleSystemRenderer PSR;
    ParticleSystemShapeType IngredientMesh = ParticleSystemShapeType.MeshRenderer;
    public Material FireSpell, AirSpell, WaterSpell, EarthSpell, LightSpell, DarkSpell;

    public int strength;
    public Elements element;
    
    protected virtual void Awake()
    {
        //strength = Random.Range(0, 3);
        strength = 4;
        element = Elements.None;
    }

    private void OnTriggerEnter(Collider collision)
    {
        //if (!Burned && !Gassed && !Drenched && !Dusted && !Shining && !Darkened)
        //{
        if (element==Elements.None){
            switch (collision.gameObject.tag)
            {
                case "Spell/Fire":
                    Debug.Log(this.name + " was hit by Fire");
                    //Burned= true;
                    element = Elements.Fire;
                    StartParticles(collision.gameObject.GetComponent<MeshRenderer>().material);
                    break;

                case "Spell/Air":
                    Debug.Log(this.name + " was hit by Air");
                    //Gassed = true;
                    element = Elements.Air;
                    //StartParticles(AirSpell);
                    StartParticles(collision.gameObject.GetComponent<MeshRenderer>().material);
                    break;

                case "Spell/Water":
                    Debug.Log(this.name + " was hit by Water");
                    //Drenched = true;
                    element = Elements.Water;
                    //StartParticles(WaterSpell);
                    StartParticles(collision.gameObject.GetComponent<MeshRenderer>().material);
                    break;

                case "Spell/Earth":
                    Debug.Log(this.name + " was hit by Earth");
                    //Dusted= true;
                    element = Elements.Earth;
                    //StartParticles(EarthSpell);
                    StartParticles(collision.gameObject.GetComponent<MeshRenderer>().material);
                    break;

                case "Spell/Light":
                    Debug.Log(this.name + " was hit by Light");
                    //Shining= true;
                    element = Elements.Light;
                    //StartParticles(LightSpell);
                    StartParticles(collision.gameObject.GetComponent<MeshRenderer>().material);
                    break;

                case "Spell/Dark":
                    Debug.Log(this.name + " was hit by Dark");
                    //Darkened= true;
                    element = Elements.Dark;
                    //StartParticles(DarkSpell);
                    StartParticles(collision.gameObject.GetComponent<MeshRenderer>().material);
                    break;

                default:
                    Debug.Log("Unknown");
                    break;

            }
        }
    }
    //}

    void StartParticles(Material ElementHit)
    {
        Debug.Log(gameObject.name + " has " + ElementHit.name);
        ParticleSystem PS = gameObject.AddComponent<ParticleSystem>();
        ParticleSystemRenderer PSR = gameObject.GetComponent<ParticleSystemRenderer>();
        PSR.material= ElementHit;
        //PS.startColor = color;
        //PSR.material= ElementHit;
        //PS.Play();
        //PS.shape.meshRenderer = gameObject.GetComponent<MeshRenderer>();

    }
}
