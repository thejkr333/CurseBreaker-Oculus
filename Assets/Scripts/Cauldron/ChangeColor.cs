using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    Material material;
    Color initialColor, targetColor;
    float timeToChangeColor = 2;
    // Start is called before the first frame update
    void Start()
    {
        //Cauldron.IngredientIn += CombineColors;
        Cauldron.IngredientIn += NewTargetColor;
        Cauldron.ClearCauldron += ResetColor;

        if (TryGetComponent(out MeshRenderer meshRenderer))
        {
            material = meshRenderer.material;
        }
        else if(TryGetComponent(out ParticleSystemRenderer psRenderer))
        {
            material = psRenderer.material;
        }
        initialColor = material.color;
    }

    IEnumerator Co_ChangeColors(Color targetColor)
    {
        float lerpValue = 0;
        while (lerpValue < 1)
        {
            material.color = Color.Lerp(material.color, targetColor, lerpValue);
            lerpValue += Time.deltaTime / timeToChangeColor;
            yield return null;
        }
        material.color = targetColor;
    }

    void NewTargetColor(Color newColor)
    {
        targetColor = newColor;
        StartCoroutine(Co_ChangeColors(targetColor));
    }

    void CombineColors(List<Color> colors)
    {
        targetColor = new Color(0, 0, 0, 0);
        foreach (Color c in colors)
        {
            targetColor += c;
        }
        targetColor /= colors.Count;

        StartCoroutine(Co_ChangeColors(targetColor));
    }

    void ResetColor()
    {
        material.color = initialColor;
    }
}
