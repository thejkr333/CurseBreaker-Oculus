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

public class SubmenuHandTrackingButton : MonoBehaviour, GestureManagerButton
{
    [System.Serializable]
    public enum Operation {
        LeftHandTrackingPoints,
        RightHandTrackingPoints,
    };
    public Operation operation;

    [SerializeField] private bool forward;

    private SubmenuHandTracking submenuHandTracking;

    [SerializeField] private Material inactiveButtonMaterial;
    [SerializeField] private Material activeButtonMaterial;

    // Start is called before the first frame update
    void Start()
    {
        this.submenuHandTracking = this.transform.parent.gameObject.GetComponent<SubmenuHandTracking>();
    }

    private void OnTriggerEnter(Collider other)
    {
        GestureManager gm = GestureManagerVR.me?.gestureManager;
        if (!other.name.EndsWith("pointer") || gm == null || gm.gc == null)
            return;
        if (GestureManagerVR.isGesturing)
            return;
        if (GestureManagerVR.activeButton != null)
            return;
        GestureManagerVR.activeButton = this;
        this.GetComponent<Renderer>().material = activeButtonMaterial;
        switch (this.operation)
        {
            case Operation.LeftHandTrackingPoints: {
                    if (this.forward) {
                        gm.leftHandTrackingPoints = (MivryQuestHands.TrackingPoints)(((int)gm.leftHandTrackingPoints + 1) % (int)MivryQuestHands.TrackingPoints._NUM_VALUES);
                    } else {
                        gm.leftHandTrackingPoints = (MivryQuestHands.TrackingPoints)(((int)gm.leftHandTrackingPoints - 1 + (int)MivryQuestHands.TrackingPoints._NUM_VALUES) % (int)MivryQuestHands.TrackingPoints._NUM_VALUES);
                    }
                }
                break;
            case Operation.RightHandTrackingPoints: {
                    if (this.forward) {
                        gm.rightHandTrackingPoints = (MivryQuestHands.TrackingPoints)(((int)gm.rightHandTrackingPoints + 1) % (int)MivryQuestHands.TrackingPoints._NUM_VALUES);
                    } else {
                        gm.rightHandTrackingPoints = (MivryQuestHands.TrackingPoints)(((int)gm.rightHandTrackingPoints - 1 + (int)MivryQuestHands.TrackingPoints._NUM_VALUES) % (int)MivryQuestHands.TrackingPoints._NUM_VALUES);
                    }
                }
                break;
        }
        GestureManagerVR.setInputFocus(null);
        this.submenuHandTracking.refresh();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name.EndsWith("pointer") && (Object)GestureManagerVR.activeButton == this)
            GestureManagerVR.activeButton = null;
        this.GetComponent<Renderer>().material = inactiveButtonMaterial;
    }

    private void OnDisable()
    {
        if ((Object)GestureManagerVR.activeButton == this)
            GestureManagerVR.activeButton = null;
        this.GetComponent<Renderer>().material = inactiveButtonMaterial;
    }
}
