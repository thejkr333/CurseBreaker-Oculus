using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatEasterEgg : MonoBehaviour
{ private bool PlayOnce;
    private float RatTimer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.GetComponent<OVRGrabbable>().isGrabbed == true && PlayOnce == false)
        {
            PlayOnce = true;
            AudioManager.Instance.PlayEasterEgg("Fabrice");
        }
        if (PlayOnce == true) RatTimer += Time.deltaTime;
        if (RatTimer > 18) AudioManager.Instance.StopEasterEgg();
    }
}
