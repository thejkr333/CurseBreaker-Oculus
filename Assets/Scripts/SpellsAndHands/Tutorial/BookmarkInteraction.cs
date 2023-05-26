using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookmarkInteraction : MonoBehaviour
{
    public int PageToSet;
    public GameObject Book;

    private BookInteraction bookScript;
    // Start is called before the first frame update
    void Awake()
    {
        if (bookScript == null && Book != null)
        {
            bookScript = Book.GetComponentInParent<BookInteraction>();
        }
    }

    public void JumpToPage()
    {
        if (bookScript!=null)//&& if the it is the bookmark interaction specified with the hand
        {
            AudioManager.Instance.PlaySoundStatic("Flipping_pages", transform.position);
            StartCoroutine(bookScript.TurnPages(PageToSet, 0.5f));
        }
    }
}
