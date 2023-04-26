using System.Collections;
using System.Collections.Generic;
using UnityEditor.XR.LegacyInputHelpers;
using UnityEngine;
using UnityEngine.UI;
public class HeightSlider : MonoBehaviour
{
    public GameObject CameraRig;
    Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.value = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //CameraRig.transform.position = new Vector3(CameraRig.transform.position.x, slider.value , CameraRig.transform.position.z);
    }

    public void SetHeight(float heightAdjustment)
    {
        CameraRig.transform.position = new Vector3(CameraRig.transform.position.x, heightAdjustment*.1f, CameraRig.transform.position.z);
    }
}
