using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookInteraction : MonoBehaviour
{
    public List<GameObject> Pages;
    int currentPage;

    bool rightPageTouched, leftPageTouched;
    float timeToTurnPage, timer;
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
        if(other.TryGetComponent(out PoseGrab poseGrab))
        {
            if (handTouched == null) handTouched = poseGrab.gameObject;
            else if (handTouched != poseGrab.gameObject)
            {
                ResetTouch();
                return;
            }
            else
            {
                if (leftPageTouched)
                {
                    NextPage();
                    ResetTouch();
                    return;
                }
                else
                {
                    rightPageTouched = true;
                    timer = timeToTurnPage;
                }
            }
        }
    }

    public void LeftPageTouched(Collider other)
    {
        if (other.TryGetComponent(out PoseGrab poseGrab))
        {
            if (handTouched == null) handTouched = poseGrab.gameObject;
            else if (handTouched != poseGrab.gameObject)
            {
                ResetTouch();
                return;
            }
            else
            {
                if (rightPageTouched)
                {
                    PreviousPage();
                    ResetTouch();
                    return;
                }
                else
                {
                    leftPageTouched = true;
                    timer = timeToTurnPage;
                }
            }
        }
    }
}
