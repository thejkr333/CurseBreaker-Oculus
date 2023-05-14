using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vial : MonoBehaviour
{
    [SerializeField] float timeForCompletion = 1;
    [SerializeField] GameObject liquid;
    SphereCollider sphereCollider;
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
                Debug.LogWarning("Submerged for: " + submergeTime + " time");
            }
            else
            {
                //submergeTime = 0;
            }
            yield return null;
        }
        liquid.SetActive(true);
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Cauldron cauldron))
        {
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
}
