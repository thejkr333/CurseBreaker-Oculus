using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndexTip : MonoBehaviour
{
    [SerializeField] private float radius;

    SphereCollider sphereCollider;
    // Start is called before the first frame update
    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.radius = radius;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out BookmarkInteraction bookmarkInteraction))
        {
            bookmarkInteraction.JumpToPage();
        }
        else if(other.TryGetComponent(out Bubble bubble))
        {
            bubble.Pop();
        }
    }
}
