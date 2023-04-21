using System;
using TMPro;
using UnityEngine;

public class DayManager : MonoBehaviour
{
    public static DayManager Instance;

    [Header("CUSTOMERS")]
    [SerializeField] GameObject customerPrefab;
    const int NUMBEROFCUSTOMERSPERDAY = 3;
    [SerializeField] GameObject[] customersToday = new GameObject[NUMBEROFCUSTOMERSPERDAY];
    [SerializeField] Transform customerPosition;
    int customerIndex;

    public Action CustomersFinished;

    [SerializeField] GameObject dayCanvas;
    [SerializeField] TMP_Text dayText;

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

        dayCanvas.SetActive(false);
        GameManager.Instance.OnNewDay += NewDay;
    }

    private void NewDay()
    {
        ActivateCanvas();
        CreateCustomers();
        Invoke(nameof(DeactivateCanvas), 2);
    }

    void ActivateCanvas()
    {
        dayText.text = "Day " + GameManager.Instance.DayCount;
        dayCanvas.SetActive(true);
    }

    void DeactivateCanvas()
    {
        dayCanvas.SetActive(false);
    }

    void CreateCustomers()
    {
        for (int i = 0; i < NUMBEROFCUSTOMERSPERDAY; i++)
        {
            customersToday[i] = Instantiate(customerPrefab);
            customersToday[i].transform.position = GameManager.Instance.Parla.position;
        }

        customersToday[0].transform.position = customerPosition.position;
    }

    public void NextCustomer()
    {
        CustomerOut(customersToday[customerIndex]);
        customerIndex++;

        if (customerIndex == 2) CustomersFinished?.Invoke();
        else CustomerIn(customersToday[customerIndex]);
    }

    void CustomerOut(GameObject customer)
    {
        Destroy(customer);
    }

    void CustomerIn(GameObject customer)
    {
        customer.transform.position = customerPosition.position;
    }
}
