using System;
using UnityEngine;

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance;

    public int Gold;
    
    [SerializeField] int paymentIncrement, rent, rentIncrement;

    public event Action Transaction;

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

    public void GainGold()
    {
        Gold += 10 + paymentIncrement;
        Transaction?.Invoke();
    }
    public void SubstractGold(int cost)
    {
        Gold -= cost;
        Transaction?.Invoke();
    }
    public void SellIngredient(int cost, Vector3 originPos)
    {
        Gold += cost;
        Transaction?.Invoke();

        AudioManager.Instance.PlaySoundStatic("sell", originPos);
    }
    public void IncreaseRent()
    {
        rent += rentIncrement;
        Transaction?.Invoke();
    }
    public void SubstractRent()
    {
        if (Gold > rent)
        {
            Gold -= rent;
        }
    }
}
