using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new_scroll",menuName = "scroll")]
public class scroll_info : ScriptableObject
{
    public Sprite previewImage;
    public string previewString;
    public string description;
    public GameObject page;
    
}
