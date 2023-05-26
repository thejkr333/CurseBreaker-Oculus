using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BookInteraction : MonoBehaviour
{
    public List<GameObject> Pages;
    int currentPage;

    bool rightPageTouched, leftPageTouched;
    [SerializeField] float timeToTurnPage;
    float timer;
    GameObject handTouched;

    [SerializeField] TMP_Text pageNumber;

    // Start is called before the first frame update
    void Awake()
    {
        SetPage(0);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) { PreviousPage(); }
        if (Input.GetKeyDown(KeyCode.N)) { NextPage(); }

        if (rightPageTouched || leftPageTouched)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                ResetTouch();
            }
        }
    }

    private void UpdatePageNumber()
    {
        pageNumber.text = (currentPage + 1).ToString() + "/" + Pages.Count;
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
        if (currentPage >= 1)
        {
            AudioManager.Instance.PlaySoundStatic("turn_page", transform.position);
            currentPage--;
            foreach (GameObject obj in Pages) { obj.SetActive(false); }
            Pages[currentPage].SetActive(true);
            UpdatePageNumber();

        }
    }

    public void NextPage()
    {
        if (currentPage < Pages.Count - 1)
        {
            AudioManager.Instance.PlaySoundStatic("turn_page", transform.position);
            currentPage++;
            foreach (GameObject obj in Pages) { obj.SetActive(false); }
            Pages[currentPage].SetActive(true);
            UpdatePageNumber();

        }
    }
    public void SetPage(int pageToSet)
    {
        currentPage = pageToSet;
        foreach (GameObject obj in Pages) { obj.SetActive(false); }
        Pages[currentPage].SetActive(true);
        UpdatePageNumber();

    }

    public void RightPageTouched(Collider other)
    {
        if (other.TryGetComponent(out PoseGrab poseGrab))
        {
            //First time touching
            if (handTouched == null)
            {
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
