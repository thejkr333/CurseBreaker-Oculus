using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Scroll : MonoBehaviour
{
    public scroll_info info;

    [SerializeField] TMP_Text title;
    [SerializeField] Image image;
    [SerializeField] TMP_Text description;

    public GameObject page;

    private void Awake()
    {
        title.text       = info.previewString;
        image.sprite     = info.previewImage;
        description.text = info.description;

        page = info.page;
    }
}
