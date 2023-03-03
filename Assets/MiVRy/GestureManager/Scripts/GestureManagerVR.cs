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

using System;
using UnityEngine;

public class GestureManagerVR : MonoBehaviour
{
    public static GestureManagerVR me; // singleton

    public bool followUser = true;

    public GestureManager gestureManager;

    [System.NonSerialized] public EditableTextField inputFocus = null;
    public Material inputFocusOnMaterial;
    public Material inputFocusOffMaterial;
    public GameObject keyboard;
    public GameObject pointerLeft;
    public GameObject pointerRight;
    public float gripThreshold = 0.5f;

    [System.NonSerialized] public GameObject submenuHandTracking = null;
    [System.NonSerialized] public GameObject submenuFiles = null;
    [System.NonSerialized] public GameObject submenuFileSuggestions = null;
    [System.NonSerialized] public GameObject submenuGesture = null;
    // [System.NonSerialized] public GameObject submenuCombination = null;
    [System.NonSerialized] public GameObject submenuRecord = null;
    [System.NonSerialized] public GameObject submenuGestureTrigger = null;
    [System.NonSerialized] public GameObject submenuFrameOfReference = null;
    [System.NonSerialized] public GameObject submenuTraining = null;

    public static GestureManagerButton activeButton = null;

    public class SampleDisplay
    {
        public int sampleId = -1;
        public int dataPointIndex = 0;
        public GameObject headsetModel = null;
        public GameObject controllerModelLeft = null;
        public GameObject controllerModelRight = null;
        public struct Stroke
        {
            public Vector3[] hmd_p;
            public Quaternion[] hmd_q;
            public Vector3[] p;
            public Quaternion[] q;
        };
        public Stroke strokeLeft;
        public Stroke strokeRight;
        public void reloadStrokes()
        {
            int ret;
            if (this.sampleId < 0) {
                strokeRight.p = null;
                strokeRight.q = null;
                strokeRight.hmd_p = null;
                strokeRight.hmd_q = null;
                strokeLeft.p = null;
                strokeLeft.q = null;
                strokeLeft.hmd_p = null;
                strokeLeft.hmd_q = null;
                return;
            }
            var gm = GestureManagerVR.me.gestureManager;
            if (gm.gc == null) {
                return;
            }
            // Right
            int gestureIdRight = gm.record_combination_id;
            if (gestureIdRight < 0 || gestureIdRight >= gm.gc.numberOfGestures(gm.rightHandPartsMin)) {
                strokeRight.p = null;
                strokeRight.q = null;
                strokeRight.hmd_p = null;
                strokeRight.hmd_q = null;
            } else {
                int sampleIdRight = Math.Min(this.sampleId, gm.gc.getGestureNumberOfSamples(gm.rightHandPartsMin, gestureIdRight) - 1);
                if (sampleIdRight < 0) {
                    strokeRight.p = null;
                    strokeRight.q = null;
                    strokeRight.hmd_p = null;
                    strokeRight.hmd_q = null;
                } else {
                    ret = gm.gc.getGestureSampleStroke(gm.rightHandPartsMin, gestureIdRight, sampleIdRight, 0,
                        ref this.strokeRight.p, ref this.strokeRight.q, ref this.strokeRight.hmd_p, ref this.strokeRight.hmd_q
                    );
                    if (ret <= 0) {
                        gm.consoleText = $"Failed to get sample data ({ret}).";
                        return;
                    }
                    for (int i = 0; i < this.strokeRight.p.Length; i++) {
                        MivryQuestHands.convertBackHandInput(gm.mivryCoordinateSystem, gm.unityXrPlugin, ref this.strokeRight.p[i], ref this.strokeRight.q[i]);
                        MivryQuestHands.convertBackHeadInput(gm.mivryCoordinateSystem, ref this.strokeRight.hmd_p[i], ref this.strokeRight.hmd_q[i]);
                    }
                }
            }
            // Left
            int gestureIdLeft = gm.record_combination_id;
            if (gestureIdLeft < 0 || gestureIdLeft >= gm.gc.numberOfGestures(gm.leftHandPartsMin)) {
                strokeLeft.p = null;
                strokeLeft.q = null;
                strokeLeft.hmd_p = null;
                strokeLeft.hmd_q = null;
            } else {
                int sampleIdLeft = Math.Min(this.sampleId, gm.gc.getGestureNumberOfSamples(gm.leftHandPartsMin, gestureIdLeft) - 1);
                if (sampleIdLeft < 0) {
                    strokeLeft.p = null;
                    strokeLeft.q = null;
                    strokeLeft.hmd_p = null;
                    strokeLeft.hmd_q = null;
                } else {
                    ret = gm.gc.getGestureSampleStroke(gm.leftHandPartsMin, gestureIdLeft, sampleIdLeft, 0,
                        ref this.strokeLeft.p, ref this.strokeLeft.q, ref this.strokeLeft.hmd_p, ref this.strokeLeft.hmd_q
                    );
                    if (ret <= 0) {
                        gm.consoleText = $"Failed to get sample data ({ret}).";
                        return;
                    }
                    for (int i = 0; i < this.strokeLeft.p.Length; i++) {
                        MivryQuestHands.convertBackHandInput(gm.mivryCoordinateSystem, gm.unityXrPlugin, ref this.strokeLeft.p[i], ref this.strokeLeft.q[i]);
                        MivryQuestHands.convertBackHeadInput(gm.mivryCoordinateSystem, ref this.strokeLeft.hmd_p[i], ref this.strokeLeft.hmd_q[i]);
                    }
                }
            }
        }
    };
    public static SampleDisplay sampleDisplay = new SampleDisplay();

    // Start is called before the first frame update
    void Start()
    {
        me = this;
        inputFocus = null;
        keyboard?.SetActive(false);

        Material inactive_pointer_material = (Material)Resources.Load("GesturemanagerInactivePointerMaterial", typeof(Material));
        this.pointerLeft.GetComponent<MeshRenderer>().material = inactive_pointer_material;
        this.pointerRight.GetComponent<MeshRenderer>().material = inactive_pointer_material;

        for (int i=0; i<this.transform.childCount; i++)
        {
            GameObject child = this.transform.GetChild(i).gameObject;
            switch (child.name)
            {
                case "SubmenuHandTracking":
                    submenuHandTracking = child;
                    break;
                case "SubmenuGestureTrigger":
                    submenuGestureTrigger = child;
                    break;
                case "SubmenuFiles":
                    submenuFiles = child;
                    break;
                case "SubmenuFileSuggestions":
                    submenuFileSuggestions = child;
                    break;
                case "SubmenuGesture":
                    submenuGesture = child;
                    break;
                // case "SubmenuCombination":
                //     submenuCombination = child;
                //     break;
                case "SubmenuRecord":
                    submenuRecord = child;
                    break;
                case "SubmenuFrameOfReference":
                    submenuFrameOfReference = child;
                    break;
                case "SubmenuTraining":
                    submenuTraining = child;
                    break;
                case "SampleDisplayHeadset":
                    sampleDisplay.headsetModel = child;
                    break;
            }
            for (int k=0; k<child.transform.childCount; k++)
            {
                GameObject grandChild = child.transform.GetChild(k).gameObject;
                EditableTextField editableTextField = grandChild.GetComponent<EditableTextField>();
                if (editableTextField != null)
                    editableTextField.refreshText();
            }
        }
        refresh();
    }

    public static void keyboardInput(KeyboardKey key)
    {
        if (me == null || me.inputFocus == null)
            return;
        me.inputFocus.keyboardInput(key);
        GestureManagerVR.refresh();
    }

    public static void setInputFocus(EditableTextField editableTextField)
    {
        if (me == null)
            return;
        if (me.inputFocus != null)
        {
            MeshRenderer meshRenderer = me.inputFocus.gameObject?.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.material = me.inputFocusOffMaterial;
            }
        }
        me.inputFocus = editableTextField;
        if (me.inputFocus != null)
        {
            MeshRenderer meshRenderer = me.inputFocus.gameObject?.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.material = me.inputFocusOnMaterial;
            }
        }
        if (me.keyboard != null)
        {
            if (me.inputFocus == null)
            {
                me.keyboard.SetActive(false);
            } else
            {
                me.keyboard.SetActive(true);
                KeyboardKey.activeKeyboardKey = null;
                if (GestureManagerHandle.draggingHandle == null || GestureManagerHandle.draggingHandle.target != GestureManagerHandle.Target.Keyboard) {
                    if ((me.inputFocus.gameObject.transform.position - me.keyboard.transform.position).magnitude > 0.4) {
                        me.keyboard.transform.rotation = Quaternion.LookRotation(Camera.main.transform.position - me.inputFocus.gameObject.transform.position) * Quaternion.AngleAxis(90.0f, Vector3.right);

                        Bounds objBounds = new Bounds(me.inputFocus.gameObject.transform.position, Vector3.zero);
                        foreach (Renderer r in me.inputFocus.gameObject.GetComponentsInChildren<Renderer>())
                        {
                            objBounds.Encapsulate(r.bounds);
                        }
                        Bounds keyboardBounds = new Bounds(me.keyboard.transform.position, Vector3.zero);
                        foreach (Renderer r in me.keyboard.GetComponentsInChildren<Renderer>())
                        {
                            keyboardBounds.Encapsulate(r.bounds);
                        }
                        me.keyboard.transform.position = new Vector3(
                            objBounds.center.x,
                            objBounds.center.y - (objBounds.extents.y + keyboardBounds.extents.y + 0.05f),
                            objBounds.center.z
                        ) + (me.keyboard.transform.up * 0.1f);
                    }
                }
            }
        }
    }

    public static void refresh()
    {
        if (me == null)
            return;
        
        if (me.gestureManager.numberOfParts <= 0)
        {
            me.submenuHandTracking.SetActive(true);
            me.submenuFiles.SetActive(false);
            me.submenuFileSuggestions.SetActive(false);
            me.submenuGesture.SetActive(false);
            me.submenuRecord.SetActive(false);
            me.submenuGestureTrigger.SetActive(false);
            me.submenuGestureTrigger.GetComponent<SubmenuGestureTrigger>().refresh();
            me.submenuFrameOfReference.SetActive(false);
            me.submenuTraining.SetActive(false);
        } else {
            me.submenuHandTracking.SetActive(true);
            me.submenuHandTracking.GetComponent<SubmenuHandTracking>().refresh();
            me.submenuFiles.SetActive(true);
            me.submenuFiles.GetComponent<SubmenuFiles>().refresh();
            me.submenuFileSuggestions.SetActive(true);
            me.submenuFileSuggestions.GetComponent<SubmenuFileSuggestions>().refresh();
            me.submenuGesture.SetActive(true);
            me.submenuGesture.GetComponent<SubmenuGesture>().refresh();
            me.submenuRecord.SetActive(true);
            me.submenuRecord.GetComponent<SubmenuRecord>().refresh();
            me.submenuGestureTrigger.SetActive(true);
            me.submenuGestureTrigger.GetComponent<SubmenuGestureTrigger>().refresh();
            me.submenuFrameOfReference.SetActive(true);
            me.submenuFrameOfReference.GetComponent<SubmenuFrameOfReference>().refresh();
            me.submenuTraining.SetActive(true);
            me.submenuTraining.GetComponent<SubmenuTraining>().refresh();
            me.submenuRecord.transform.localPosition = Vector3.zero;
            me.submenuGestureTrigger.transform.localPosition = Vector3.zero;
            me.submenuFrameOfReference.transform.localPosition = Vector3.zero;
            me.submenuTraining.transform.localPosition = Vector3.zero;
        }
        var editableTextFields = FindObjectsOfType<EditableTextField>();
        foreach (var editableTextField in editableTextFields)
        {
            if (editableTextField.gameObject.activeSelf)
            {
                editableTextField.refreshText();
            }
        }
    }

    public static void refreshTextInputs(GameObject go)
    {
        for (int i=0; i<go.transform.childCount; i++)
        {
            GameObject child = go.transform.GetChild(i).gameObject;
            EditableTextField field = child.GetComponent<EditableTextField>();
            if (field != null)
                field.refreshText();
        }
    }

    private void Update()
    {
        if (followUser) {
            Vector3 v = Camera.main.transform.worldToLocalMatrix.MultiplyPoint3x4(this.transform.position);
            if (v.magnitude > 0.6f || v.z < 0)
            {
                v = new Vector3(0, 0, 0.5f);
                v = Camera.main.transform.localToWorldMatrix.MultiplyPoint3x4(v);
                this.transform.position = 0.9f * this.transform.position + 0.1f * v;
                Vector3 lookDir = Camera.main.transform.position - this.transform.position;
                lookDir.y = 0; // not facing up or down
                this.transform.rotation = Quaternion.LookRotation(lookDir) * Quaternion.AngleAxis(180.0f, Vector3.up) * Quaternion.AngleAxis(-90.0f, Vector3.right);
            }
        }
        this.updatePointer(this.pointerLeft, gestureManager.left_hand, gestureManager.left_hand_skeleton);
        this.updatePointer(this.pointerRight, gestureManager.right_hand, gestureManager.right_hand_skeleton);
        if (GestureManagerHandle.draggingHandle != null && Time.time - GestureManagerHandle.draggingHandleLastUpdate > 1) {
            GestureManagerHandle.draggingHandle = null;
        }
        if (GestureManagerHandle.hoverHandle != null && Time.time - GestureManagerHandle.hoverHandleLastUpdate > 1) {
            GestureManagerHandle.hoverHandle = null;
            GestureManagerVR.gesturingEnabled = true;
        }
        if (sampleDisplay.sampleId < 0) {
            if (sampleDisplay.controllerModelRight != null && sampleDisplay.controllerModelRight.activeSelf)
            {
                sampleDisplay.controllerModelRight.SetActive(false);
            }
            if (sampleDisplay.controllerModelLeft != null && sampleDisplay.controllerModelLeft.activeSelf)
            {
                sampleDisplay.controllerModelLeft.SetActive(false);
            }
            if (sampleDisplay.headsetModel != null && sampleDisplay.headsetModel.activeSelf)
            {
                sampleDisplay.headsetModel.SetActive(false);
            }
        } else {
            if (sampleDisplay.controllerModelRight == null)
            {
                var rightHand = GameObject.Find("Right Hand");
                sampleDisplay.controllerModelRight = UnityEngine.Object.Instantiate(rightHand, this.transform);
                sampleDisplay.controllerModelRight.name = "sampleDisplay.controllerModelRight";
                Destroy(sampleDisplay.controllerModelRight.GetComponent<UnityEngine.SpatialTracking.TrackedPoseDriver>());
                for (int i = sampleDisplay.controllerModelRight.transform.childCount - 1; i>=0; i--) {
                    var child = sampleDisplay.controllerModelRight.transform.GetChild(i);
                    
                    for (int j = child.childCount-1; j>=0; j--) {
                        var grandChild = child.GetChild(j);
                        if (grandChild.gameObject.name.EndsWith("pointer")) {
                            GameObject.Destroy(grandChild.gameObject);
                        } else {
                            MeshRenderer meshRenderer = grandChild.gameObject.GetComponent<MeshRenderer>();
                            if (meshRenderer != null)
                                meshRenderer.material = sampleDisplay.headsetModel?.GetComponent<MeshRenderer>()?.material;
                        }
                    }
                }
            }
            if (sampleDisplay.controllerModelLeft == null) {
                var leftHand = GameObject.Find("Left Hand");
                sampleDisplay.controllerModelLeft = UnityEngine.Object.Instantiate(leftHand, this.transform);
                sampleDisplay.controllerModelLeft.name = "sampleDisplay.controllerModelLeft";
                Destroy(sampleDisplay.controllerModelLeft.GetComponent<UnityEngine.SpatialTracking.TrackedPoseDriver>());
                for (int i = sampleDisplay.controllerModelLeft.transform.childCount - 1; i >= 0; i--) {
                    var child = sampleDisplay.controllerModelLeft.transform.GetChild(i);
                    for (int j = child.childCount - 1; j >= 0; j--) {
                        var grandChild = child.GetChild(j);
                        if (grandChild.gameObject.name.EndsWith("pointer")) {
                            GameObject.Destroy(grandChild.gameObject);
                        } else {
                            MeshRenderer meshRenderer = grandChild.gameObject.GetComponent<MeshRenderer>();
                            if (meshRenderer != null)
                                meshRenderer.material = sampleDisplay.headsetModel?.GetComponent<MeshRenderer>()?.material;
                        }
                    }
                }
            }

            int numDpi = 0;
            if (sampleDisplay.strokeRight.p != null && sampleDisplay.strokeRight.p.Length > 0) {
                numDpi = sampleDisplay.strokeRight.p.Length;
                int dpi = Math.Min(sampleDisplay.strokeRight.p.Length - 1, Math.Max(0, sampleDisplay.dataPointIndex));
                sampleDisplay.controllerModelRight.SetActive(true);
                sampleDisplay.controllerModelRight.transform.position = sampleDisplay.strokeRight.p[dpi];
                sampleDisplay.controllerModelRight.transform.rotation = sampleDisplay.strokeRight.q[dpi];
                sampleDisplay.headsetModel.SetActive(true);
                sampleDisplay.headsetModel.transform.position = sampleDisplay.strokeRight.hmd_p[dpi];
                sampleDisplay.headsetModel.transform.rotation = sampleDisplay.strokeRight.hmd_q[dpi];
            } else {
                sampleDisplay.controllerModelRight.SetActive(false);
            }
            if (sampleDisplay.strokeLeft.p != null && sampleDisplay.strokeLeft.p.Length > 0) {
                numDpi = Math.Max(numDpi, sampleDisplay.strokeLeft.p.Length);
                int dpi = Math.Min(sampleDisplay.strokeLeft.p.Length - 1, Math.Max(0, sampleDisplay.dataPointIndex));
                sampleDisplay.controllerModelLeft.SetActive(true);
                sampleDisplay.controllerModelLeft.transform.position = sampleDisplay.strokeLeft.p[dpi];
                sampleDisplay.controllerModelLeft.transform.rotation = sampleDisplay.strokeLeft.q[dpi];
                sampleDisplay.headsetModel.SetActive(true);
                sampleDisplay.headsetModel.transform.position = sampleDisplay.strokeLeft.hmd_p[dpi];
                sampleDisplay.headsetModel.transform.rotation = sampleDisplay.strokeLeft.hmd_q[dpi];
            } else {
                sampleDisplay.controllerModelLeft.SetActive(false);
            }

            sampleDisplay.dataPointIndex++;
            int fps = Math.Max(10, (int)(1.0f / Time.deltaTime));
            if (sampleDisplay.dataPointIndex > numDpi + fps)
            { // allowing for one second before and after
                sampleDisplay.dataPointIndex = -fps;
            }
        }
    }

    public static bool isGesturing
    {
        get {
            if (me == null || me.gestureManager == null)
                return false;
            return me.gestureManager.gesture_started;
        }
    }

    public static bool gesturingEnabled
    {
        get
        {
            if (me == null || me.gestureManager == null)
                return false;
            return me.gestureManager.gesturing_enabled;
        }
        set
        {
            if (me == null || me.gestureManager == null)
                return;
            me.gestureManager.gesturing_enabled = value;
        }
    }

    public static bool setSubmenuGesture(int gesture_id)
    {
        if (me == null || me.submenuGesture == null)
            return false;
        GestureManagerVR.me.submenuGesture.GetComponent<SubmenuGesture>().CurrentGesture = gesture_id;
        GestureManagerVR.me.submenuGesture.GetComponent<SubmenuGesture>().refresh();
        return true;
    }
    public static int getSubmenuCombination()
    {
        if (me.submenuGesture != null) {
            return GestureManagerVR.me.submenuGesture.GetComponent<SubmenuGesture>().CurrentGesture;
        }
        return -1;
    }

    public static bool setSubmenuCombination(int combination_id, int part=-1, int gesture_id=-1)
    {
        // if (me == null || me.submenuCombination == null || me.gestureManager == null || me.gestureManager.gc == null)
        //     return false;
        // if (gesture_id<0)
        // {
        //     for (part = me.gestureManager.gc.numberOfParts()-1; part >=0 ; part--)
        //     {
        //         gesture_id = me.gestureManager.gc.getCombinationPartGesture(combination_id, part);
        //         if (gesture_id >= 0)
        //             break;
        //     }
        // }
        // GestureManagerVR.me.submenuCombination.GetComponent<SubmenuCombination>().CurrentCombination = combination_id;
        // GestureManagerVR.me.submenuCombination.GetComponent<SubmenuCombination>().CurrentPart = part;
        // GestureManagerVR.me.submenuCombination.GetComponent<SubmenuCombination>().CurrentGesture = gesture_id;
        // GestureManagerVR.me.submenuCombination.GetComponent<SubmenuCombination>().refresh();
        // if (me.submenuGesture != null)
        // {
        //     GestureManagerVR.me.submenuGesture.GetComponent<SubmenuGesture>().CurrentGesture = gesture_id;
        //     GestureManagerVR.me.submenuGesture.GetComponent<SubmenuGesture>().CurrentPart = part;
        //     GestureManagerVR.me.submenuGesture.GetComponent<SubmenuGesture>().refresh();
        // }
        return true;
    }

    public bool isPointing(OVRSkeleton handSkeleton)
    {
        if (handSkeleton.GetCurrentNumBones() < (int)OVRSkeleton.BoneId.Hand_End || handSkeleton.Bones.Count < (int)OVRSkeleton.BoneId.Hand_End) {
            return false;
        }
        Quaternion wristRotation = handSkeleton.Bones[(int)OVRSkeleton.BoneId.Hand_WristRoot].Transform.rotation;
        float indexBend = Quaternion.Angle(
            wristRotation,
            handSkeleton.Bones[(int)OVRSkeleton.BoneId.Hand_Index2].Transform.rotation
        );
        if (indexBend > 60.0f) {
            return false;
        }
        float middleBend = Quaternion.Angle(
            wristRotation,
            handSkeleton.Bones[(int)OVRSkeleton.BoneId.Hand_Middle2].Transform.rotation
        );
        if (middleBend < 90.0f) {
            return false;
        }
        float ringBend = Quaternion.Angle(
            wristRotation,
            handSkeleton.Bones[(int)OVRSkeleton.BoneId.Hand_Ring2].Transform.rotation
        );
        if (ringBend < 90.0f) {
            return false;
        }
        float pinkyBend = Quaternion.Angle(
            wristRotation,
            handSkeleton.Bones[(int)OVRSkeleton.BoneId.Hand_Pinky2].Transform.rotation
        );
        if (pinkyBend < 90.0f) {
            return false;
        }
        return true;
    }

    private void updatePointer(GameObject pointer, OVRHand hand, OVRSkeleton hand_skeleton)
    {
        if (hand_skeleton.GetCurrentNumBones() < (int)OVRSkeleton.BoneId.Hand_End || hand_skeleton.Bones.Count < (int)OVRSkeleton.BoneId.Hand_End) {
            pointer.SetActive(false);
        } else {
            pointer.SetActive(true);
            pointer.transform.position = hand_skeleton.Bones[(int)OVRSkeleton.BoneId.Hand_IndexTip].Transform.position;
            pointer.transform.rotation = hand_skeleton.Bones[(int)OVRSkeleton.BoneId.Hand_WristRoot].Transform.rotation;
        }
        // if (this.isPointing(hand_skeleton)) {
        //     pointer.SetActive(true);
        //     pointer.transform.position = hand_skeleton.Bones[(int)OVRSkeleton.BoneId.Hand_IndexTip].Transform.position;
        //     pointer.transform.rotation = hand_skeleton.Bones[(int)OVRSkeleton.BoneId.Hand_WristRoot].Transform.rotation;
        // } else if (hand.GetFingerIsPinching(OVRHand.HandFinger.Index)) {
        //     pointer.SetActive(true);
        //     pointer.transform.position = hand_skeleton.Bones[(int)OVRSkeleton.BoneId.Hand_IndexTip].Transform.position; // Hand_ThumbTip
        //     pointer.transform.rotation = hand_skeleton.Bones[(int)OVRSkeleton.BoneId.Hand_WristRoot].Transform.rotation;
        // } else {
        //     pointer.SetActive(false);
        // }
    }
}
