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

public class SubmenuGestureTriggerButton : MonoBehaviour, GestureManagerButton
{
    public bool leftHand;
    public bool forward;
    public TextMesh gestureTriggerDisplay;

    [SerializeField] private Material inactiveButtonMaterial;
    [SerializeField] private Material activeButtonMaterial;

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
        int gesture_trigger_mod = (int)MivryQuestHands.GestureTrigger._NUM_VALUES;
        if (this.leftHand) {
            int gesture_trigger = (int)gm.left_gesture_trigger;
            if (forward) {
                gesture_trigger = (gesture_trigger + 1) % gesture_trigger_mod;
            } else {
                gesture_trigger = (gesture_trigger == 0)
                    ? gesture_trigger_mod - 1
                    : gesture_trigger - 1;
            }
            gm.left_gesture_trigger = (MivryQuestHands.GestureTrigger)gesture_trigger;
            gestureTriggerDisplay.text = gm.left_gesture_trigger.ToString();
        } else { // right hand
            int gesture_trigger = (int)gm.right_gesture_trigger;
            if (forward) {
                gesture_trigger = (gesture_trigger + 1) % gesture_trigger_mod;
            } else {
                gesture_trigger = (gesture_trigger == 0)
                    ? gesture_trigger_mod - 1
                    : gesture_trigger - 1;
            }
            gm.right_gesture_trigger = (MivryQuestHands.GestureTrigger)gesture_trigger;
            gestureTriggerDisplay.text = gm.right_gesture_trigger.ToString();
        }
        GestureManagerVR.setInputFocus(null);
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