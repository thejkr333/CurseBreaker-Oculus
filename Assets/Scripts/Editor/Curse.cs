using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curse : ScriptableObject
{
    public List<step> steps = new List<step>();
    public GameObject effect;

    step s = new step();

    public void do_something()
    {
        //s.get_potion()
    }
}

public class step
{


    public Potion p;
    public Gesture g;

    public void get_potion(Potion p, int i)
    {
        
    }
    public void get_potion(Potion p, float f)
    {

    }
}