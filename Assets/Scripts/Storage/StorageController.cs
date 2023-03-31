using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageController : MonoBehaviour
{
    public int MandrakeAmount, AngelLeafAmount, WolfsBaneAmount, CorkWoodAmount;
    public Transform MandrakeStorage, AngelLeafStorage, WolfsBaneStorage, CorkWoodStorage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Mandrake>())
        {
            MandrakeAmount++;
            other.transform.position = MandrakeStorage.position;
        }
        if (other.gameObject.GetComponent<AngelLeaf>())
        {
            AngelLeafAmount++;
            other.transform.position = AngelLeafStorage.position;
        }
        if (other.gameObject.GetComponent<CorkWood>())
        {
            CorkWoodAmount++;
            other.transform.position = CorkWoodStorage.position;
        }
        if (other.gameObject.GetComponent<WolfsBane>())
        {
            WolfsBaneAmount++;
            other.transform.position = WolfsBaneStorage.position;
        }
    }
}
