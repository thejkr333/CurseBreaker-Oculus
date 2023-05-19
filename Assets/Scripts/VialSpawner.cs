using UnityEngine;

public class VialSpawner : MonoBehaviour
{
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] GameObject objectToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("spawnPoints.Length1 " + spawnPoints.Length);
        for (int x = 0; x < spawnPoints.Length; x++)
        {
            SpawnVial();
        }
    }

    public void SpawnVial()
    {
        bool spawned = false;
        for (int x = 0; x < spawnPoints.Length && !spawned; x++)
        {
            if(spawnPoints[x].childCount == 0)
            {
                GameObject clon = Instantiate(objectToSpawn, spawnPoints[x].position, Quaternion.identity);
                clon.transform.SetParent(spawnPoints[x].transform, true);
                clon.GetComponent<Rigidbody>().isKinematic = true;
                clon.GetComponent<Vial>().onDestroy += SpawnVial;
                spawned = true;
            }
        }
    }
}