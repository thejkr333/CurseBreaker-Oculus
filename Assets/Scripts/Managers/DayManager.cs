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

    GameObject currentCustomer;

    public int SunState;
    public Action customerCured, nextCustomer, customerOut;
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
        //GameManager.Instance.OnNewDay += NewDay;
        currentCustomer = Instantiate(customerPrefab);
        currentCustomer.GetComponent<CustomerFloat>().spawn = customerPosition;
        CustomerIn(currentCustomer);
        //GameManager.Instance.NextDay();
    }

    private void Update()
    {
        //Declare SunState for the SunController
        SunState = customerIndex;

        //Skip debug
        if (Input.GetKeyDown(KeyCode.K)) NextCustomer();
    }

    private void NewDay()
    {
        customerIndex = 0;
        GoldManager.Instance.Gold += 40;
        ActivateCanvas();

        CreateCustomers();
      
        Invoke(nameof(DeactivateCanvas), 5);
        GoldManager.Instance.ResetDayBalances();
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

        CustomerIn(customersToday[0]);
    }

    public void NextCustomer()
    {
        //if (customerIndex > NUMBEROFCUSTOMERSPERDAY - 1) return;

        //CustomerOut(customersToday[customerIndex]);
        CustomerOut(currentCustomer);

        //if (customerIndex++ == NUMBEROFCUSTOMERSPERDAY - 1) CustomersFinished?.Invoke();
        //else CustomerIn(customersToday[customerIndex]);
        currentCustomer = Instantiate(customerPrefab);
        currentCustomer.GetComponent<CustomerFloat>().spawn = customerPosition;
        CustomerIn(currentCustomer);

        nextCustomer?.Invoke();
    }

    void CustomerOut(GameObject customer)
    {
        Destroy(customer);
        customerOut?.Invoke();
    }

    void CustomerIn(GameObject customer)
    {
        customer.GetComponent<Rigidbody>().velocity = Vector3.zero;
        customer.transform.parent = customerPosition;
        customer.transform.position = customerPosition.position;
        customer.transform.eulerAngles = new Vector3(0, customerPosition.eulerAngles.y, 0);
    }

    public void ResetCustomerPos()
    {
        //if (customerIndex > 2) return;

        //customersToday[customerIndex].transform.position = customerPosition.position;
        //customersToday[customerIndex].transform.eulerAngles = new Vector3(0, customerPosition.eulerAngles.y, 0); 
        currentCustomer.GetComponent<Rigidbody>().velocity = Vector3.zero;
        currentCustomer.transform.position = customerPosition.position;
        currentCustomer.transform.eulerAngles = new Vector3(0, customerPosition.eulerAngles.y, 0);
    }

    public void CustomerCured()
    {
        customerCured?.Invoke();
        Invoke(nameof(NextCustomer), 2);
    }
}
