using System.Collections.Generic;
using UnityEngine;

public enum Elements { Fire, Dark, Light, Water, Air, Earth, Void }
public class Potion : MonoBehaviour
{
    public List<Ingredients> PotionIngredients = new();

    public List<Elements> PotionElements = new();

    [HideInInspector] public GameObject elementParticles;

    [HideInInspector] public Gradient gradient;
    List<GradientColorKey> gradientColorKeys = new();
    List<GradientAlphaKey> gradientAlphaKeys = new();

    private void Start()
    {
        gradient.mode = GradientMode.Fixed;
    }
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
                AddColorToGradient(Elements.Fire);
                break;

            case "Spell/Air":
                PotionElements.Add(Elements.Air);
                AddColorToGradient(Elements.Air);
                break;

            case "Spell/Water":
                PotionElements.Add(Elements.Water);
                AddColorToGradient(Elements.Water);
                break;

            case "Spell/Earth":
                PotionElements.Add(Elements.Earth);
                AddColorToGradient(Elements.Earth);
                break;

            case "Spell/Light":
                PotionElements.Add(Elements.Light);
                AddColorToGradient(Elements.Light);
                break;

            case "Spell/Dark":
                //Darkened= true;
                PotionElements.Add(Elements.Dark);
                AddColorToGradient(Elements.Dark);
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

    void AddColorToGradient(Elements element)
    {
        Color _color = Utils.GetElementColor(element);

        GradientColorKey key = new GradientColorKey();
        key.color = _color;
        gradientColorKeys.Add(key);

        GradientColorKey[] colorKeys = gradientColorKeys.ToArray();
        for (int i = 0; i < colorKeys.Length; i++)
        {
            colorKeys[i].time = (float)(i + 1) / colorKeys.Length;
            gradientAlphaKeys.Add(new GradientAlphaKey());
        }

        GradientAlphaKey[] alphaKeys = gradientAlphaKeys.ToArray();
        for (int i = 0; i < gradientAlphaKeys.Count; i++)
        {
            alphaKeys[i].alpha = 1;
            alphaKeys[i].time = (float)(i + 1) / alphaKeys.Length;
        }

        gradient.SetKeys(colorKeys, alphaKeys);

        ColorParticles();
    }

    private void ColorParticles()
    {
        for (int i = 0; i < elementParticles.transform.childCount; i++)
        {
            var main = elementParticles.transform.GetChild(i).GetComponent<ParticleSystem>().main;
            var randomColors = new ParticleSystem.MinMaxGradient(gradient);
            randomColors.mode = ParticleSystemGradientMode.RandomColor;
            main.startColor = randomColors;

            elementParticles.transform.GetChild(i).GetComponent<ParticleSystem>().Play();
        }       
    }
}
