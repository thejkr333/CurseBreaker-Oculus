using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{


    public int Gold, Rent, RentIncrement, PaymentIncrement;
    public static GameManager Instance;
    // Start is called before the first frame update
    void Start()
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

    // Update is called once per frame
    void Update()
    {
        
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
}
