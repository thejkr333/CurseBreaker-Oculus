using UnityEngine;
//using UnityEngine.XR.Interaction.Toolkit;

public enum Elements { Fire, Dark, Light, Water, Air, Earth, None }
//[RequireComponent(typeof(XRGrabInteractable), typeof(Renderer), typeof(Rigidbody))]
public class Ingredient : MonoBehaviour
{
    public bool Burned, Gassed, Drenched, Dusted, Shining, Darkened;
    public enum Ingredients { AngelLeaf, Mandrake, WolfsBane, CorkWood }
    [HideInInspector] public Ingredients ingredient;

    //bool hasBeenSelected;

    [HideInInspector] public bool selected;

    //XRBaseInteractable m_Interactable;
    //Renderer m_Renderer;
    Rigidbody rb;

    public int strength;
    public Elements element;
    
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = true;

        element = Elements.None;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!Burned && !Gassed && !Drenched && !Dusted && !Shining && !Darkened)
        {
            switch (collision.gameObject.tag)
            {
                case "Spell/Fire":
                    Debug.Log(this.name + " was hit by Fire");
                    Burned= true;
                    element = Elements.Fire;
                    StartParticles(Color.red);
                    break;

                case "Spell/Air":
                    Debug.Log(this.name + " was hit by Air");
                    Gassed = true;
                    element = Elements.Air;
                    break;

                case "Spell/Water":
                    Debug.Log(this.name + " was hit by Water");
                    Drenched = true;
                    element = Elements.Water;
                    break;

                case "Spell/Earth":
                    Debug.Log(this.name + " was hit by Earth");
                    Dusted= true;
                    element = Elements.Earth;
                    break;

                case "Spell/Light":
                    Debug.Log(this.name + " was hit by Light");
                    Shining= true;
                    element = Elements.Light;
                    break;

                case "Spell/Dark":
                    Debug.Log(this.name + " was hit by Dark");
                    Darkened= true;
                    element = Elements.Dark;
                    break;

                default:
                    Debug.Log("Unknown");
                    break;

            }
        }
    }

    void StartParticles(Color color)
    {

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
