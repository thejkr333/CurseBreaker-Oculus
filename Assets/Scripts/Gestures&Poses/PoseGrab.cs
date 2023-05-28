using System.Collections;
using UnityEngine;

public class PoseGrab : OVRGrabber
{
    // Boolean used to check if is Grabbing or not.
    [SerializeField]
    bool isGrabbing = false;
    public bool SpellRelease;

    OVRHand hand;
    protected override void Start()
    {
        // We begin by initialize the base.Start function where are set few variable from OVRGrabber like:
        // m_lastPos, m_last_Rot and the m_parentTransform.
        base.Start();

        hand = GetComponent<OVRHand>();
    }

    // Function used as a switch to determinate if we are grabbing or not by passing as argument
    // the string "true" in the gesture when detected and "false" when is not recognize
    public void DetectGrabbing(bool _isGrabbing)
    {
        //// if "_isGrabbing" is true, we set isGrabbing to true
        //if (_isGrabbing.Equals("true"))
        //{
        //    isGrabbing = true;
        //}
        //// else if "_isGrabbing" is false, we set isGrabbing to false
        //else if (_isGrabbing.Equals("false"))
        //{
        //    isGrabbing = false;
        //}
        isGrabbing = _isGrabbing;
        SpellRelease = !isGrabbing;
    }

    public override void Update()
    {
        if(!hand.IsTracked)
        {
            //isGrabbing = false;
            //IsReleasing();
            //GrabEnd();
            return;
        }

        // we call the base.Update to make sure that OVRGrabber update some values
        base.Update();
        //Debug.Log("Update R linear velocity: " + transform);
        
        // if we are not grabbin anything and we have a candidate able to be grabbed
        // and isGrabbing (found by the gesture detector on this case) is true
        if (!m_grabbedObj && m_grabCandidates.Count > 0 && isGrabbing)
        {
            // we call the GrabBegin the object
            GrabBeginToPos();
        }
        // else if there is an object that we are grabbing and the isGrabbing is false
        else if (m_grabbedObj != null && !isGrabbing)
        {
            // we call the override GrabEnd
            GrabEnd();
        }

        m_lastPos = transform.position;
        m_lastRot = transform.rotation;

    }

    // To call in the gestures to refresh the position and rotation when releasing
    public void IsReleasing()
    {
        m_lastPos = transform.position;
        m_lastRot = transform.rotation;
    }

    protected override void GrabEnd()
    {
        // if there is an object we are grabbing
        if (m_grabbedObj != null)
        {
            // we calculate the linearVelocity calculate by:
            // transform.parent.position (position of --HandAnchor) minus the last position recorded
            // everything divided by time.fixedDeltaTime
            //Vector3 linearVelocity = (transform.parent.position - m_lastPos) / Time.fixedDeltaTime;
            double vX = (double)transform.position.x - (double)m_lastPos.x;
            double vY = (double)transform.position.y - (double)m_lastPos.y;
            double vZ = (double)transform.position.z - (double)m_lastPos.z;

            Vector3 linearVelocity = (transform.position - (m_lastPos)) / Time.deltaTime;
            //Vector3 linearVelocity = OVRInput.GetLocalControllerVelocity(m_controller); //This doesnt work because ony supports OCULUS controllers
            // the same operation is calculated but in this case is calculated on the EulerAngles
            Vector3 angularVelocity = (transform.eulerAngles - m_lastRot.eulerAngles) / Time.deltaTime;
            //Vector3 angularVelocity = OVRInput.GetLocalControllerAngularVelocity(m_controller); //This doesnt work because ony supports OCULUS controllers

            if (m_grabbedObj.TryGetComponent(out AlwaysLookToCam lookToCam))
            {
                lookToCam.enabled = true;
            }

            // And we call the function that make us able to release the grab with the velocities we calculated          
            GrabbableRelease(linearVelocity, angularVelocity);
        }

        // And the we restore de collider used for the grabbing
        GrabVolumeEnable(true);
    }
}
