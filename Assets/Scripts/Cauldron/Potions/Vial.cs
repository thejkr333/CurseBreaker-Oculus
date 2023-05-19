using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vial : MonoBehaviour
{
    [SerializeField] float timeForCompletion = 1;
    [SerializeField] GameObject liquid;
    SphereCollider sphereCollider;

    [SerializeField] GameObject elementParticles;
    [SerializeField] Gradient gradient;

    public Action onDestroy;
    // Start is called before the first frame update
    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    IEnumerator Co_CheckSubmerge(Collider cauldronCol)
    {
        bool totallySubmerged = false;
        float submergeTime = 0;
        while (!totallySubmerged || submergeTime < timeForCompletion)
        {
            totallySubmerged = CheckBounds(cauldronCol);
            if (totallySubmerged)
            {
                submergeTime += Time.deltaTime;
                if(!AudioManager.Instance.IsSoundPlaying("Fill_Potion")) 
                    AudioManager.Instance.PlaySoundStaticAtTime("Fill_Potion", transform.position, (submergeTime / timeForCompletion) * .01f);
            }
            else
            {
                if(AudioManager.Instance.IsSoundPlaying("Fill_Potion")) AudioManager.Instance.StopSound("Fill_Potion");
            }
            yield return null;
        }
        if (cauldronCol.TryGetComponent(out Cauldron cauldron))
        {
            liquid.GetComponent<MeshRenderer>().material.color = Utils.PotionDoneColor;
            liquid.SetActive(true);
            ConvertToPotion(cauldron);
        }
    }

    bool CheckBounds(Collider container)
    {
        Vector3[] pointsToCheck = new Vector3[6];
        pointsToCheck[0] = new Vector3(sphereCollider.bounds.center.x, sphereCollider.bounds.center.y, sphereCollider.bounds.center.z + sphereCollider.radius);
        pointsToCheck[1] = new Vector3(sphereCollider.bounds.center.x, sphereCollider.bounds.center.y, sphereCollider.bounds.center.z - sphereCollider.radius);
        pointsToCheck[2] = new Vector3(sphereCollider.bounds.center.x, sphereCollider.bounds.center.y + sphereCollider.radius, sphereCollider.bounds.center.z);
        pointsToCheck[3] = new Vector3(sphereCollider.bounds.center.x, sphereCollider.bounds.center.y - sphereCollider.radius, sphereCollider.bounds.center.z);
        pointsToCheck[4] = new Vector3(sphereCollider.bounds.center.x + sphereCollider.radius, sphereCollider.bounds.center.y, sphereCollider.bounds.center.z);
        pointsToCheck[5] = new Vector3(sphereCollider.bounds.center.x - sphereCollider.radius, sphereCollider.bounds.center.y, sphereCollider.bounds.center.z);

        for (int i = 0; i < pointsToCheck.Length; i++)
        {
            if (!container.bounds.Contains(pointsToCheck[i])) return false;
        }

        return true;
    }

    void ConvertToPotion(Cauldron cauldron)
    {
        cauldron.ResetCauldron();
        Potion _potion = gameObject.AddComponent<Potion>();
        _potion.CreatePotion(cauldron.IngredientsInCauldron);
        _potion.elementParticles = elementParticles;
        _potion.gradient = gradient;
        this.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Cauldron cauldron))
        {
            if (!cauldron.PotionDone) return;
            StartCoroutine(Co_CheckSubmerge(other));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Cauldron cauldron))
        {
            StopCoroutine(Co_CheckSubmerge(other));
        }
    }

    private void OnDestroy()
    {
        onDestroy?.Invoke();
    }
}
