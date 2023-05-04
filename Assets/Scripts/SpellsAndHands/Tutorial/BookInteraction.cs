using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookInteraction : MonoBehaviour
{
    public List<GameObject> Pages;
    int currentPage;

    bool rightPageTouched, leftPageTouched;
    [SerializeField] float timeToTurnPage;
    float timer;
    GameObject handTouched;

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

        if(rightPageTouched || leftPageTouched) 
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                ResetTouch();
            }
        }
    }

    private void ResetTouch()
    {
        Debug.Log("AA Reset");
        handTouched = null;
        rightPageTouched = false;
        leftPageTouched = false;
        timer = timeToTurnPage;
    }

    public void PreviousPage()
    {
        AudioManager.Instance.PlaySoundStatic("turn_page", transform.position);
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
        AudioManager.Instance.PlaySoundStatic("turn_page", transform.position);
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

    public void RightPageTouched(Collider other)
    {
        if (other.TryGetComponent(out PoseGrab poseGrab))
        {
            //First time touching
            if (handTouched == null)
            {
                Debug.Log("AA 1st time right");
                handTouched = poseGrab.gameObject;

                rightPageTouched = true;
                timer = timeToTurnPage;
                return;
            }

            //Check if its the same hand touching it
            if (handTouched != poseGrab.gameObject)
            {
                ResetTouch();
                return;
            }

            if (leftPageTouched)
            {
                PreviousPage();
                ResetTouch();
                return;
            }
        }
    }

    public void LeftPageTouched(Collider other)
    {
        if (other.TryGetComponent(out PoseGrab poseGrab))
        {
            if (handTouched == null)
            {
                Debug.Log("AA 1st time left");
                handTouched = poseGrab.gameObject;

                leftPageTouched = true;
                timer = timeToTurnPage;
                return;
            }
            
            if (handTouched != poseGrab.gameObject)
            {
                ResetTouch();
                return;
            }

            if (rightPageTouched)
            {
                NextPage();
                ResetTouch();
                return;
            }
        }
    }
}
