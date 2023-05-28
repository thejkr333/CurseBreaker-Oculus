using Oculus.Interaction.Samples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            try
            {
                player = GameObject.Find("CenterEyeAnchor");
            } catch { Debug.LogWarning("Player not found."); }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            gameObject.transform.LookAt(player.transform.position);
        }
    }
}
