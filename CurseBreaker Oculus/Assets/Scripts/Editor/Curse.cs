using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curse : ScriptableObject
{
    public List<step> steps = new List<step>();
    public GameObject effect;

    


}

public class step
{
    public Potion p;
    public Gesture g;
}