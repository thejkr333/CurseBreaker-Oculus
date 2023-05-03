using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnTriggerDelegator : MonoBehaviour
{
    public EventSensor Enter;
    public EventSensor Exit;

    /// <summary>
    /// Evento de trigger enter
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        Enter.Invoke(other);
    }
    private void OnTriggerExit(Collider other)
    {
        Exit.Invoke(other);
    }

    [System.Serializable]
    public class EventSensor : UnityEvent<Collider>
    {
    }
}