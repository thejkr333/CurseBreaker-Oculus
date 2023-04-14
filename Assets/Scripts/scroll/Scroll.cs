using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Scroll : MonoBehaviour
{
    scroll_info info;

    TMP_Text text;
    Image image;

    public GameObject page;
    //private GameObject background = GameObject.CreatePrimitive(PrimitiveType.Plane);

    private void Awake()
    {
        text.text = info.previewString;
        image.sprite = info.previewImage;

        //background.transform.localScale = new Vector3(0.01f, 0.01f, 0.015f);
        //background.transform.parent = this.transform;
    }
}
