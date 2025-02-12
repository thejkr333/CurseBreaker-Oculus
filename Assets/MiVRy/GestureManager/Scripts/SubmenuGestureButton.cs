﻿/*
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

public class SubmenuGestureButton : MonoBehaviour, GestureManagerButton
{
    [System.Serializable]
    public enum Operation {
        NextPart,
        PreviousPart,
        CreateGesture,
        DeleteGesture,
        DeleteLastSample,
        DeleteAllSamples,
        NextGesture,
        PreviousGesture,
        TrackedHandNext,
        TrackedHandBack
    };
    public Operation operation;

    private SubmenuGesture submenuGesture;

    [SerializeField] private Material inactiveButtonMaterial;
    [SerializeField] private Material activeButtonMaterial;

    // Start is called before the first frame update
    void Start()
    {
        this.submenuGesture = this.transform.parent.gameObject.GetComponent<SubmenuGesture>();
    }

    private void OnTriggerEnter(Collider other)
    {
        GestureManager gm = GestureManagerVR.me?.gestureManager;
        if (!other.name.EndsWith("pointer") || gm == null)
            return;
        if (GestureManagerVR.isGesturing)
            return;
        if (GestureManagerVR.activeButton != null)
            return;
        GestureManagerVR.activeButton = this;
        this.GetComponent<Renderer>().material = activeButtonMaterial;
        if (gm.gc == null)
            return;
        switch (this.operation)
        {
            case Operation.CreateGesture:
                this.submenuGesture.CurrentGesture = gm.createGesture("New Gesture");
                if (gm.record_combination_id >= 0) {
                    gm.record_combination_id = this.submenuGesture.CurrentGesture;
                }
                break;
            case Operation.DeleteGesture:
                if (this.submenuGesture.CurrentGesture >= 0)
                {
                    gm.deleteGesture(this.submenuGesture.CurrentGesture);
                    this.submenuGesture.CurrentGesture--;
                    if (gm.record_combination_id >= 0) {
                        gm.record_combination_id = this.submenuGesture.CurrentGesture;
                    }
                }
                break;
            case Operation.DeleteLastSample:
                if (this.submenuGesture.CurrentGesture >= 0)
                {
                    int numSamples = gm.gc.getGestureNumberOfSamples(0, this.submenuGesture.CurrentGesture);
                    if (numSamples <= 0)
                        return;
                    for (int part = gm.gc.numberOfParts() - 1; part >= 0; part--) {
                        gm.gc.deleteGestureSample(part, this.submenuGesture.CurrentGesture, numSamples - 1);
                    }
                }
                break;
            case Operation.DeleteAllSamples:
                if (this.submenuGesture.CurrentGesture >= 0)
                {
                    for (int part = gm.gc.numberOfParts() - 1; part >= 0; part--) {
                        gm.gc.deleteAllGestureSamples(part, this.submenuGesture.CurrentGesture);
                    }
                }
                break;
            case Operation.NextGesture:
                {
                    int numGestures = gm.gc.numberOfGestureCombinations();
                    if (numGestures == 0) {
                        this.submenuGesture.CurrentGesture = -1;
                    } else if (this.submenuGesture.CurrentGesture + 1 >= numGestures) {
                        this.submenuGesture.CurrentGesture = 0;
                    } else {
                        this.submenuGesture.CurrentGesture++;
                    }
                    if (gm.record_combination_id >= 0) {
                        gm.record_combination_id = this.submenuGesture.CurrentGesture;
                    }
                }
                break;
            case Operation.PreviousGesture: {
                    int numGestures = gm.gc.numberOfGestureCombinations();
                    if (numGestures == 0) {
                        this.submenuGesture.CurrentGesture = -1;
                    } else if (this.submenuGesture.CurrentGesture - 1 < 0) {
                        this.submenuGesture.CurrentGesture = numGestures - 1;
                    } else {
                        this.submenuGesture.CurrentGesture--;
                    }
                    if (gm.record_combination_id >= 0) {
                        gm.record_combination_id = this.submenuGesture.CurrentGesture;
                    }
                }
                break;
            case Operation.NextPart:
                break;
            case Operation.PreviousPart:
                break;
            case Operation.TrackedHandBack: {
                    int trackedHand = (int)gm.getTrackedHand(this.submenuGesture.CurrentGesture);
                    trackedHand = (trackedHand <= 0) ? (int)MivryQuestHands.TrackedHand._NUM_VALUES-1 : trackedHand - 1;
                    gm.setTrackedHand(this.submenuGesture.CurrentGesture, (MivryQuestHands.TrackedHand)trackedHand);
                }
                break;
            case Operation.TrackedHandNext: {
                    int trackedHand = (int)gm.getTrackedHand(this.submenuGesture.CurrentGesture);
                    trackedHand++;
                    if (trackedHand >= (int)MivryQuestHands.TrackedHand._NUM_VALUES) trackedHand = 0;
                    gm.setTrackedHand(this.submenuGesture.CurrentGesture, (MivryQuestHands.TrackedHand)trackedHand);
                }
                break;
        }
        GestureManagerVR.setInputFocus(null);
        GestureManagerVR.refresh();
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
