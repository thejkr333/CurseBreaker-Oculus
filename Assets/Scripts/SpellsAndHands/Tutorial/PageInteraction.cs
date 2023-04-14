using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageInteraction : MonoBehaviour
{
    public bool IsNextPage;

    BookInteraction tutorialBook;
    // Start is called before the first frame update
    void Awake()
    {
        tutorialBook=gameObject.GetComponentInParent<BookInteraction>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PoseGrab posegrab))
        {
            //Do something when on trigger with a hand
            if(IsNextPage)
            {
                tutorialBook.NextPage();
            }
            else
            {
                tutorialBook.PreviousPage();
            }

        }

        if(other.TryGetComponent (out Scroll scroll))
        {
            tutorialBook.Pages.Add(scroll.page);
            Destroy(scroll, 0.5f);
        }
    }
}
