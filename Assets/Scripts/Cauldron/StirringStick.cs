using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StirringStick : MonoBehaviour
{
    Vector3 initialPos, initialRot;
    Animator animator;

    float timer, offPosTime = 3;
    bool inCauldron, stirring;

    int lapCounter;

    [SerializeField] SliderController sliderController;
    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.position;
        initialRot = transform.eulerAngles;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnim();

        //if (inCauldron || stirring) { timer = 0; return; }

        //if (Vector3.Distance(transform.position, initialPos) > .5f)
        //{
        //    timer += Time.deltaTime;
        //    if (timer >= offPosTime)
        //    {
        //        transform.position = initialPos;
        //        transform.eulerAngles = initialRot;
        //        timer = 0;
        //    }
        //}
        //else timer = 0;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Cauldron>())
        {
            animator.enabled = true;
            inCauldron = true;
        }

        if (other.GetComponent<PoseGrab>())
        {
            if (inCauldron)
            {
                animator.enabled = true;
                stirring = true;

                AudioManager.Instance.PlaySoundStatic("boiling_water", transform.position);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Cauldron>())
        {
            animator.enabled = true;
            inCauldron = false;
        }
    }

    void UpdateAnim()
    {
        if (!animator.enabled) return;

        animator.SetBool("InCauldron", inCauldron);
        animator.SetBool("Stir", stirring);
    }

    public void CheckFinishStirring()
    {
        lapCounter++;
        if(lapCounter >= 2)
        {
            AudioManager.Instance.StopSound("boiling_water");
            animator.enabled = true;
            stirring = false;
            inCauldron = false;
            GetComponent<Collider>().enabled = false;
            lapCounter = 0;
            sliderController.StartMinigame();
        }
    }

    public void BackOnIni()
    {
        inCauldron = false;
        GetComponent<Collider>().enabled = true;
        animator.enabled = false;
    }


    public void DisableAnim()
    {
        animator.enabled = false;
    }
}
