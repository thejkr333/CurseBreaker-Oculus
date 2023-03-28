using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int Gold, Rent, RentIncrement, PaymentIncrement;

    [SerializeField] Transform parla;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

    }

    public void RentIncrease()
    {
        Rent += RentIncrement;
    }
   public void GoldGain()
    {
        Gold += 10 + PaymentIncrement;
    }

    public void RentisDue()
    {
        if (Gold > Rent)
        {
            Gold -= Rent;
        }
    }

    public void DestroyGrabbedThings(GameObject obj)
    {
        obj.transform.position = parla.position;
        Destroy(obj, 1f);
    }
}
