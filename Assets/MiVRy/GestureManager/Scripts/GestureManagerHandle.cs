/*
 * MiVRy - 3D gesture recognition library plug-in for Unity.
 * Version 2.7
 * Copyright (c) 2023 MARUI-PlugIn (inc.)
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS 
 * "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, 
 * THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR 
 * PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR 
 * CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, 
 * EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, 
 * PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR 
 * PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY 
 * OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT 
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE 
 * OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureManagerHandle : MonoBehaviour
{
    public enum Target
    {
        GestureManager
        ,
        Keyboard
    };
    [SerializeField] public  Target   target;
    [SerializeField] private Material inactiveHandleMaterial;
    [SerializeField] private Material hoverHandleMaterial;
    [SerializeField] private Material activeHandleMaterial;

    private GameObject activePointer = null;
    private Matrix4x4  lastPointerMat;

    public static GestureManagerHandle hoverHandle = null;
    public static GestureManagerHandle draggingHandle = null;
    public static float hoverHandleLastUpdate = 0.0f;
    public static float draggingHandleLastUpdate = 0.0f;

    private static float triggerPressureThreshold = 0.7f;
    private static float triggerPressureAlpha(float triggerPressure)
    {
        return (triggerPressure - triggerPressureThreshold) / (1.0f - triggerPressureThreshold);
    }

    private void Update()
    {
        if (draggingHandle == this) {
            OVRHand hand = (activePointer.name.ToLower().Contains("left"))
                ? GameObject.Find("LeftHandAnchor").transform.Find("OVRHandPrefab").GetComponent<OVRHand>()
                : GameObject.Find("RightHandAnchor").transform.Find("OVRHandPrefab").GetComponent<OVRHand>();
            
            float trigger_pressure = hand.GetFingerPinchStrength(OVRHand.HandFinger.Index);
            if (trigger_pressure < 0.80f) {
                draggingHandle = null;
                GestureManagerVR.gesturingEnabled = true;
                this.GetComponent<Renderer>().material = (hoverHandle == this) ? hoverHandleMaterial : inactiveHandleMaterial;
                this.lastPointerMat = Matrix4x4.identity;
                return;
            } // else:
            Matrix4x4 pointerMat = Matrix4x4.TRS(this.activePointer.transform.position, this.activePointer.transform.rotation, Vector3.one);
            if (!this.lastPointerMat.isIdentity) {
                GameObject targetObject = this.target == Target.Keyboard
                ? GestureManagerVR.me.keyboard
                : GestureManagerVR.me.gameObject;
                Matrix4x4 gmMat = Matrix4x4.TRS(
                    targetObject.transform.position,
                    targetObject.transform.rotation,
                    Vector3.one
                );
                gmMat = (pointerMat * this.lastPointerMat.inverse) * gmMat;
                Vector3 p = gmMat.GetColumn(3);
                Quaternion q = gmMat.rotation;
                float a = triggerPressureAlpha(trigger_pressure);
                targetObject.transform.position = ((targetObject.transform.position * (1.0f - a)) + (p * a));
                targetObject.transform.rotation = Quaternion.Slerp(targetObject.transform.rotation, q, a);
            }
            this.GetComponent<Renderer>().material = activeHandleMaterial;
            this.lastPointerMat = pointerMat;
            draggingHandleLastUpdate = Time.time;
            if (GestureManagerVR.me != null && GestureManagerVR.me.followUser) {
                GestureManagerVR.me.followUser = false;
                GameObject followMeButtonText = GameObject.Find("SubmenuGestureManagerFollowValue");
                TextMesh followMeButtonTextComponent = followMeButtonText?.GetComponent<TextMesh>();
                if (followMeButtonTextComponent != null)
                {
                    followMeButtonTextComponent.text = "No";
                }
            }
            return;
        }

        if (hoverHandle == this) {
            OVRHand hand = (activePointer.name.ToLower().Contains("left"))
                ? GameObject.Find("LeftHandAnchor").transform.Find("OVRHandPrefab").GetComponent<OVRHand>()
                : GameObject.Find("RightHandAnchor").transform.Find("OVRHandPrefab").GetComponent<OVRHand>();

            float trigger_pressure = hand.GetFingerPinchStrength(OVRHand.HandFinger.Index);
            if (trigger_pressure > 0.85f) {
                GestureManagerVR.gesturingEnabled = false;
                draggingHandle = this;
                draggingHandleLastUpdate = Time.time;
                this.GetComponent<Renderer>().material = activeHandleMaterial;
                this.lastPointerMat = Matrix4x4.identity;
            }
            return;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!other.name.EndsWith("pointer"))
            return;
        if (GestureManagerVR.isGesturing)
            return;
        if (hoverHandle != null)
            return;
        GestureManagerVR.gesturingEnabled = false;
        hoverHandle = this;
        activePointer = other.gameObject;
        hoverHandleLastUpdate = Time.time;
        this.GetComponent<Renderer>().material = hoverHandleMaterial;
    }

    public void OnTriggerStay(Collider other)
    {
        if (!other.name.EndsWith("pointer"))
            return;
        if (GestureManagerVR.isGesturing)
            return;
        if (hoverHandle == null) {
            this.OnTriggerEnter(other);
        } else if (hoverHandle == this) {
            hoverHandleLastUpdate = Time.time;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject != this.activePointer)
            return;
        if (hoverHandle != this)
            return;
        hoverHandle = null;
        GestureManagerVR.gesturingEnabled = true;
        this.GetComponent<Renderer>().material = inactiveHandleMaterial;
    }
}
