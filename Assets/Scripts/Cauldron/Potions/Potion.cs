using System.Collections.Generic;
using UnityEngine;

public enum Elements { Fire, Dark, Light, Water, Air, Earth, Void }
public class Potion : MonoBehaviour
{
    public List<Ingredients> PotionIngredients = new();

    public List<Elements> PotionElements = new();

    [SerializeField] GameObject elementParticles;

    [SerializeField] Color fireColor, waterColor, airColor, earthColor, lightColor, darkColor; 

    [SerializeField] Gradient gradient;
    [SerializeField] List<GradientColorKey> gradientColorKeys = new();
    List<GradientAlphaKey> gradientAlphaKeys = new();

    public int Strength;
    private void Awake()
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
                AddColorToGradient(fireColor);
                break;

            case "Spell/Air":
                PotionElements.Add(Elements.Air);
                AddColorToGradient(airColor);
                break;

            case "Spell/Water":
                PotionElements.Add(Elements.Water);
                AddColorToGradient(waterColor);
                break;

            case "Spell/Earth":
                PotionElements.Add(Elements.Earth);
                AddColorToGradient(earthColor);
                break;

            case "Spell/Light":
                PotionElements.Add(Elements.Light);
                AddColorToGradient(lightColor);
                break;

            case "Spell/Dark":
                //Darkened= true;
                PotionElements.Add(Elements.Dark);
                AddColorToGradient(darkColor);
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

    void AddColorToGradient(Color color)
    {
        GradientColorKey key = new GradientColorKey();
        key.color = color;
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
