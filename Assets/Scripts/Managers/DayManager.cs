using System;
using System.Collections;
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

    [SerializeField] int demonitisEasterEggProb = 1;
    [SerializeField] GameObject demonitisEasterEggPrefab;
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
        //GameManager.Instance.NextDay();
    }

    private void Start()
    {
        currentCustomer = Instantiate(customerPrefab);
        currentCustomer.GetComponent<CustomerFloat>().spawn = customerPosition;
        CustomerIn(currentCustomer);
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

        if (UnityEngine.Random.Range(1, 101) <= demonitisEasterEggProb)
        {
            currentCustomer = Instantiate(demonitisEasterEggPrefab);
            currentCustomer.transform.position = customerPosition.position;
            AudioManager.Instance.PlayEasterEgg("Sans");
            StartCoroutine(nameof(Co_ChangeLights));
        }
        else
        {
            currentCustomer = Instantiate(customerPrefab);
        }
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
        if(customer.TryGetComponent(out Rigidbody rb)) rb.velocity = Vector3.zero;
        customer.transform.position = customerPosition.position;
        customer.transform.eulerAngles = new Vector3(0, customerPosition.eulerAngles.y, 0);
    }

    public void ResetCustomerPos()
    {
        //if (customerIndex > 2) return;

        //customersToday[customerIndex].transform.position = customerPosition.position;
        //customersToday[customerIndex].transform.eulerAngles = new Vector3(0, customerPosition.eulerAngles.y, 0); 
        if (currentCustomer.TryGetComponent(out Rigidbody rb)) rb.velocity = Vector3.zero;
        currentCustomer.transform.position = customerPosition.position;
        currentCustomer.transform.eulerAngles = new Vector3(0, customerPosition.eulerAngles.y, 0);
    }

    public void CustomerCured()
    {
        customerCured?.Invoke();
        Invoke(nameof(NextCustomer), 2);
    }

    IEnumerator Co_ChangeLights()
    {
        Light[] _changeLights = FindObjectsOfType<Light>();
        float[] _intensities = new float[_changeLights.Length];
        Color[] _colors = new Color[_changeLights.Length];

        for (int i = 0; i < _changeLights.Length; i++)
        {
            _intensities[i] = _changeLights[i].intensity;
            _colors[i] = _changeLights[i].color;
            yield return null;
        }

        bool _intensityReached = false;
        while(!_intensityReached)
        {
            _intensityReached = true;
            for (int i = 0; i < _changeLights.Length; i++)
            {
                _changeLights[i].color = Color.red;

                if (_changeLights[i].intensity < 2)
                {
                    _changeLights[i].intensity += Time.deltaTime;
                }
                else
                {
                    _changeLights[i].intensity -= Time.deltaTime;
                }

                if (Mathf.Abs(_changeLights[i].intensity - 2) > .1f) _intensityReached = false;
            }
            yield return null;
        }
      
        yield return new WaitForSeconds(30);

        AudioManager.Instance.StopEasterEgg();

        _intensityReached = false;
        while (!_intensityReached)
        {
            _intensityReached = true;
            for (int i = 0; i < _changeLights.Length; i++)
            {
                _changeLights[i].color = _colors[i];

                if (_changeLights[i].intensity < _intensities[i])
                {
                    _changeLights[i].intensity += Time.deltaTime;
                }
                else
                {
                    _changeLights[i].intensity -= Time.deltaTime;
                }

                if (Mathf.Abs(_changeLights[i].intensity - _intensities[i]) > .01f) _intensityReached = false;
            }
            yield return null;
        }

        NextCustomer();
    }
}
