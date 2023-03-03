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

public class SubmenuGesture : MonoBehaviour
{
    private bool initialized = false;
    private GameObject GestureNextBtn;
    private GameObject GesturePrevBtn;
    private GameObject GestureNameText;
    private GameObject GestureNameInput;
    private GameObject GestureCreateBtn;
    private GameObject GestureDeleteLastSampleBtn;
    private GameObject GestureDeleteAllSamplesBtn;
    private GameObject GestureSamplesText;
    private GameObject GestureDeleteGestureBtn;
    private GameObject GestureTrackedHandText;
    private GameObject GestureTrackedHandNextBtn;
    private GameObject GestureTrackedHandValue;
    private GameObject GestureTrackedHandPrevBtn;

    private int currentGesture = -1;

    public int CurrentGesture
    {
        get { return currentGesture; }
        set { currentGesture = value; refresh(); }
    }

    void Start()
    {
        this.init();
        this.refresh();
    }

    private void init()
    {
        for (int i=0; i<this.transform.childCount; i++)
        {
            GameObject child = this.transform.GetChild(i).gameObject;
            switch (child.name)
            {
                case "SubmenuGestureNextBtn":
                    GestureNextBtn = child;
                    break;
                case "SubmenuGesturePrevBtn":
                    GesturePrevBtn = child;
                    break;
                case "SubmenuGestureNameText":
                    GestureNameText = child;
                    break;
                case "SubmenuGestureNameInput":
                    GestureNameInput = child;
                    break;
                case "SubmenuGestureCreateBtn":
                    GestureCreateBtn = child;
                    break;
                case "SubmenuGestureDeleteLastSampleBtn":
                    GestureDeleteLastSampleBtn = child;
                    break;
                case "SubmenuGestureDeleteAllSamplesBtn":
                    GestureDeleteAllSamplesBtn = child;
                    break;
                case "SubmenuGestureSamplesText":
                    GestureSamplesText = child;
                    break;
                case "SubmenuGestureDeleteGestureBtn":
                    GestureDeleteGestureBtn = child;
                    break;
                case "SubmenuGestureTrackedHandText":
                    GestureTrackedHandText = child;
                    break;
                case "SubmenuGestureTrackedHandNextBtn":
                    GestureTrackedHandNextBtn = child;
                    break;
                case "SubmenuGestureTrackedHandValue":
                    GestureTrackedHandValue = child;
                    break;
                case "SubmenuGestureTrackedHandPrevBtn":
                    GestureTrackedHandPrevBtn = child;
                    break;
            }
        }
        this.initialized = true;
    }

    public void refresh()
    {
        if (!this.initialized)
            this.init();
        GestureManager gm = GestureManagerVR.me?.gestureManager;
        if (gm == null)
            return;
        if (gm.gc == null)
        {
            return;
        }
        
        TextMesh tm;

        GestureCreateBtn.SetActive(true);
        int numGestures = gm.gc.numberOfGestureCombinations();
        if (numGestures <= 0)
        {
            GestureNextBtn.SetActive(false);
            GesturePrevBtn.SetActive(false);
            GestureNameText.SetActive(false);
            GestureNameInput.SetActive(false);
            GestureDeleteLastSampleBtn.SetActive(false);
            GestureDeleteAllSamplesBtn.SetActive(false);
            GestureSamplesText.SetActive(false);
            GestureDeleteGestureBtn.SetActive(false);
            GestureTrackedHandText.SetActive(false);
            GestureTrackedHandNextBtn.SetActive(false);
            GestureTrackedHandValue.SetActive(false);
            GestureTrackedHandPrevBtn.SetActive(false);
            return;
        }
        if (this.currentGesture < 0 || this.currentGesture >= numGestures)
            this.currentGesture = 0;
        GestureNextBtn.SetActive(true);
        GesturePrevBtn.SetActive(true);
        GestureNameText.SetActive(true);
        GestureNameInput.SetActive(true);
        GestureDeleteLastSampleBtn.SetActive(true);
        GestureDeleteAllSamplesBtn.SetActive(true);
        GestureSamplesText.SetActive(true);
        GestureDeleteGestureBtn.SetActive(true);
        tm = GestureNameText.GetComponent<TextMesh>();
        if (tm != null)
            tm.text = gm.gc.getGestureCombinationName(this.currentGesture);
        tm = GestureSamplesText.GetComponent<TextMesh>();
        MivryQuestHands.TrackedHand trackedHand = gm.getTrackedHand(this.currentGesture);
        if (tm != null) {
            if (trackedHand == MivryQuestHands.TrackedHand.LeftHand) {
                tm.text = $"{gm.gc.getGestureNumberOfSamples(gm.leftHandPartsMin, this.currentGesture)} samples";
            } else if (trackedHand == MivryQuestHands.TrackedHand.RightHand) {
                tm.text = $"{gm.gc.getGestureNumberOfSamples(gm.rightHandPartsMin, this.currentGesture)} samples";
            } else if (trackedHand == MivryQuestHands.TrackedHand.BothHands) {
                tm.text = $"{gm.gc.getGestureNumberOfSamples(gm.leftHandPartsMin, this.currentGesture)}/{gm.gc.getGestureNumberOfSamples(gm.rightHandPartsMin, this.currentGesture)} samples";
            } else {
                tm.text = "";
            }
        }
        GestureTrackedHandText.SetActive(true);
        GestureTrackedHandNextBtn.SetActive(true);
        GestureTrackedHandPrevBtn.SetActive(true);
        GestureTrackedHandValue.SetActive(true);
        tm = GestureTrackedHandValue.GetComponent<TextMesh>();
        if (tm != null) {
            if (trackedHand == MivryQuestHands.TrackedHand.LeftHand) {
                tm.text = "Left";
            } else if (trackedHand == MivryQuestHands.TrackedHand.RightHand) {
                tm.text = "Right";
            } else if (trackedHand == MivryQuestHands.TrackedHand.BothHands) {
                tm.text = "Both";
            } else {
                tm.text = "???";
            }
        }
        return;
    }
}
