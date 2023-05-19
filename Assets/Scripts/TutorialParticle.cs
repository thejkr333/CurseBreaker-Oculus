using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialParticle : MonoBehaviour
{
    [SerializeField] List<GameObject> TutorialItems = new List<GameObject>();

    [SerializeField] GameObject Tutorial;

    GameObject CurrentObject;
    Mesh CurrentMesh;
    

    // Start is called before the first frame update
    void Start()
    {
        Mesh[] _objects;
        
        foreach(GameObject o in TutorialItems)
        {
            if (o.GetComponentsInChildren<Mesh>() != null)
                AddParticleEffect(o);
                
        }
        _objects = CurrentObject.GetComponentsInChildren<Mesh>();

        CurrentMesh = CurrentObject.GetComponent<Mesh>();


        //I want to find the object I am spawning it on 

        CurrentMesh = transform.parent.GetComponent<MeshFilter>().mesh;
        //tutorial.shape.mesh.triangles = CurrentMesh.triangles;

            
    }
    void SetMeshOnObjects()
    {

    }

    void AddParticleEffect(GameObject MeshObject)
    {
        //Instantiate()
        


    }

    void TurnOnParticles()
    {

    }

    void TurnOffParticles()
    {

    }
}
