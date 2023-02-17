using UnityEngine;
//using UnityEngine.XR.Interaction.Toolkit;


//[RequireComponent(typeof(XRGrabInteractable), typeof(Renderer), typeof(Rigidbody))]
public class Ingredient : MonoBehaviour
{
    public enum Ingredients { Red, Green, Blue, Purple, Yellow }
    [HideInInspector] public Ingredients ingredient;

    //bool hasBeenSelected;

    [HideInInspector] public bool selected;

    //XRBaseInteractable m_Interactable;
    //Renderer m_Renderer;
    Rigidbody rb;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = true;
    }

    //protected void OnEnable()
    //{
    //    m_Interactable = GetComponent<XRBaseInteractable>();
    //    m_Renderer = GetComponent<Renderer>();

    //    m_Interactable.firstSelectEntered.AddListener(OnFirstSelectEntered);
    //    m_Interactable.lastSelectExited.AddListener(OnLastSelectExited);
    //}

    //protected void OnDisable()
    //{
    //    m_Interactable.firstSelectEntered.RemoveListener(OnFirstSelectEntered);
    //    m_Interactable.lastSelectExited.RemoveListener(OnLastSelectExited);
    //}

    //protected virtual void OnFirstSelectEntered(SelectEnterEventArgs args) => EnterSelection();

    //protected virtual void OnLastSelectExited(SelectExitEventArgs args) => ExitSelection();

    //protected virtual void EnterSelection()
    //{
    //    selected = true;
    //    rb.isKinematic = false;
    //    if (hasBeenSelected) return;

    //    Instantiate(gameObject, transform.position, Quaternion.identity);
    //    hasBeenSelected = true;
    //}
    //protected virtual void ExitSelection()
    //{
    //    rb.isKinematic = false;
    //    selected = false;
    //}
}
