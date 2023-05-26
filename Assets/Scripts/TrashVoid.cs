using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashVoid : MonoBehaviour
{
    [SerializeField] float range, attractionForce;
    [SerializeField] GameObject destructionParticles;

    private void FixedUpdate()
    {
        CheckRadius();
    }

    void CheckRadius()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, range);
        if (cols.Length == 0) return;

        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].TryGetComponent(out Trashable trashable))
            {
                if (!trashable.CanBeDestroyed) return;

                float _distance = Vector3.Distance(transform.position, trashable.transform.position);
                if (_distance > .25f)
                {
                    Attract(trashable.gameObject);
                }
                else
                {
                    Instantiate(destructionParticles, trashable.transform.position, trashable.transform.rotation);
                    Destroy(trashable.gameObject);
                }
            }
        }
    }

    void Attract(GameObject go)
    {
        if(!go.TryGetComponent(out Rigidbody rb)) rb = go.AddComponent<Rigidbody>();

        rb.useGravity = false;
        Vector3 _direction = transform.position - go.transform.position;
        rb.AddForce(_direction.normalized * attractionForce, ForceMode.Force);
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
