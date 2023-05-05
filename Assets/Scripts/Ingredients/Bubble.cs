using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : Noise
{
    [HideInInspector] public Ingredient IngredientInside;
    Transform target, tr;
    Vector3 posMov;
    [HideInInspector] public Transform[] WayPoints;
    [HideInInspector] public float MaxMoveDistance;
    [HideInInspector] public float RotSpeed;
    [HideInInspector] public BubbleManager BubbleManager;

    Quaternion targetRot;
    int rotX, rotY, rotZ;

    protected override void Start()
    {
        base.Start();

        tr = transform;
        targetRot = Random.rotation;
        posMov = tr.position;
        target = ChooseRandomPoint();

        rotX = Random.Range(-1, 2);
        rotY = Random.Range(-1, 2);
        rotZ = Random.Range(-1, 2);
    }
    protected override void Update()
    {
        IncrementManagement();

        RotateRandom();

        if (target == null) return;

        Vector3 _noise = CalculateNoise();

        if (Vector3.Distance(tr.position, target.position) > .5f)
        {
            posMov = Vector3.MoveTowards(posMov, target.position, MaxMoveDistance);

            tr.position = posMov + _noise;
        }
        else
        {
            target = ChooseRandomPoint();
        }
    }

    Transform ChooseRandomPoint()
    {
        int _randomPoint = Random.Range(0, WayPoints.Length);
        return WayPoints[_randomPoint];
    }

    void RotateRandom()
    {
        //Random rotations
        //if (1 - Mathf.Abs(Quaternion.Dot(tr.rotation, targetRot)) > .5f)
        //{
        //    tr.rotation = Quaternion.Slerp(tr.rotation, targetRot, RotSpeed * Time.deltaTime);
        //}
        //else targetRot = Random.rotation;

        tr.Rotate(new Vector3(rotX, rotY, rotZ) * RotSpeed * Time.deltaTime);
    }

    public void Pop()
    {
        BubbleManager.PopBubble(IngredientInside.ThisIngredient);
    }
}
