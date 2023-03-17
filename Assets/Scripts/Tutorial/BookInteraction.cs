using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookInteraction : MonoBehaviour
{

    public List<GameObject> Pages;
    int currentPage;
    // Start is called before the first frame update
    void Awake()
    {
        SetPage(0);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P)) { PreviousPage(); }
        if(Input.GetKeyDown(KeyCode.N)) { NextPage(); }
    }

    public void PreviousPage()
    {
        if (currentPage >= 1)
        {
            currentPage--;
            foreach (GameObject obj in Pages) { obj.SetActive(false); }
            Pages[currentPage].SetActive(true);
            Debug.Log("Current Page is " + (currentPage + 1) + " of " + Pages.Count);
        }
    }

    public void NextPage()
    {
        if (currentPage < Pages.Count-1)
        {
            currentPage++;
            foreach(GameObject obj in Pages) { obj.SetActive(false); }
            Pages[currentPage].SetActive(true);
            Debug.Log("Current Page is " + (currentPage+1) + " of " + Pages.Count);
        }

    }
    public void SetPage(int pageToSet)
    {
        currentPage = pageToSet;
        foreach (GameObject obj in Pages) { obj.SetActive(false); }
        Pages[currentPage].SetActive(true);
        Debug.Log("Current Page is " + (currentPage+1) + " of " + Pages.Count);
    }
}
