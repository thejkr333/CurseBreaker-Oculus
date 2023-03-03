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
using System;
using System.Runtime.InteropServices;
using System.IO;
using AOT;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.Networking;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
#endif

public class GestureManager : MonoBehaviour
{
    public int leftHandPartsMin
    {
        get
        {
            return 0;
        }
    }
    public int leftHandPartsMax
    {
        get
        {
            return (int)OVRSkeleton.BoneId.Hand_End - 1; // 24
        }
    }
    public int rightHandPartsMin
    {
        get
        {
            return (int)OVRSkeleton.BoneId.Hand_End;
        }
    }
    public int rightHandPartsMax
    {
        get
        {
            return (2 * (int)OVRSkeleton.BoneId.Hand_End) - 1; // 24
        }
    }

    // Fields to be controlled by the editor:
    public int numberOfParts {
        get
        {
            return 2 * (int)OVRSkeleton.BoneId.Hand_End; // 24;
        }
    }

    public string license_id = "";
    public string license_key = "";
    public string license_file_path = "";
    private bool license_activated = false;

    [SerializeField] private MivryQuestHands.TrackedHand default_tracked_hand = MivryQuestHands.TrackedHand.BothHands;
    public MivryQuestHands.TrackedHand getTrackedHand(int combo_id)
    {
        if (combo_id < 0 || this.gc == null || combo_id >= this.gc.numberOfGestureCombinations()) {
            return this.default_tracked_hand;
        }
        string metadata = this.gc.getGestureCombinationMetadataAsString(combo_id);
        if (metadata != null && metadata != "") {
            string[] metadataParts = metadata.Split(' ');
            if (metadataParts[0] == "MivryQuestHands" && metadataParts.Length == 2) {
                metadataParts = metadataParts[1].Split('=');
                if (metadataParts[0] == "TrackedHand" && metadataParts.Length == 2) {
                    MivryQuestHands.TrackedHand tracked_hand;
                    if (Enum.TryParse<MivryQuestHands.TrackedHand>(metadataParts[1], true, out tracked_hand)) {
                        return tracked_hand;
                    }
                }
            }
        }
        Debug.LogWarning($"[MivryQuestHands.GestureManager.getTrackedHand] failed to parse tracked hand from metadata '{metadata}'");
        if (this.gc.getCombinationPartGesture(combo_id, this.leftHandPartsMin) >=0) {
            if (this.gc.getCombinationPartGesture(combo_id, this.rightHandPartsMin) >=0) {
                return MivryQuestHands.TrackedHand.BothHands;
            } else {
                return MivryQuestHands.TrackedHand.LeftHand;
            }
        } else {
            if (this.gc.getCombinationPartGesture(combo_id, this.rightHandPartsMin) >= 0) {
                return MivryQuestHands.TrackedHand.RightHand;
            } else {
                Debug.LogError($"[MiVRy.GestureManager.getTrackedHand] Neither hand is tracked for gesture {combo_id} '{this.gc.getGestureCombinationName(combo_id)}'");
                return this.default_tracked_hand;
            }
        }
    }
    public bool setTrackedHand(int combo_id, MivryQuestHands.TrackedHand trackedHand)
    {
        if (combo_id < 0 || this.gc == null || combo_id >= this.gc.numberOfGestureCombinations()) {
            return false;
        }
        string metadata = $"MivryQuestHands TrackedHand={trackedHand.ToString()}";
        this.gc.setGestureCombinationMetadataAsString(combo_id, metadata);
        if (trackedHand == MivryQuestHands.TrackedHand.LeftHand || trackedHand == MivryQuestHands.TrackedHand.BothHands) {
            for (int part = this.leftHandPartsMin; part <= this.leftHandPartsMax; part++) {
                if (this.gc.getPartEnabled(part)) {
                    this.gc.setCombinationPartGesture(combo_id, part, combo_id);
                    this.gc.setGestureEnabled(part, combo_id, true);
                } else {
                    this.gc.setCombinationPartGesture(combo_id, part, -1);
                    this.gc.setGestureEnabled(part, combo_id, false);
                }
            }
        } else {
            for (int part = this.leftHandPartsMin; part <= this.leftHandPartsMax; part++) {
                this.gc.setCombinationPartGesture(combo_id, part, -1);
                this.gc.setGestureEnabled(part, combo_id, false);
            }
        }
        if (trackedHand == MivryQuestHands.TrackedHand.RightHand || trackedHand == MivryQuestHands.TrackedHand.BothHands) {
            for (int part = this.rightHandPartsMin; part <= this.rightHandPartsMax; part++) {
                if (this.gc.getPartEnabled(part)) {
                    this.gc.setCombinationPartGesture(combo_id, part, combo_id);
                    this.gc.setGestureEnabled(part, combo_id, true);
                } else {
                    this.gc.setCombinationPartGesture(combo_id, part, -1);
                    this.gc.setGestureEnabled(part, combo_id, false);
                }
            }
        } else {
            for (int part = this.rightHandPartsMin; part <= this.rightHandPartsMax; part++) {
                this.gc.setCombinationPartGesture(combo_id, part, -1);
                this.gc.setGestureEnabled(part, combo_id, false);
            }
        }
        return true;
    }

    [SerializeField] private MivryQuestHands.TrackingPoints left_hand_tracking_points = MivryQuestHands.TrackingPoints.AllFingerTips;
    public MivryQuestHands.TrackingPoints leftHandTrackingPoints
    {
        get
        {
            return this.left_hand_tracking_points;
        }
        set
        {
            this.left_hand_tracking_points = value;
            this.gc.setMetadataAsString($"MivryQuestHands LeftHandTrackingPoints={this.left_hand_tracking_points.ToString()} RightHandTrackingPoints={this.right_hand_tracking_points.ToString()}");
            HashSet<int> active_parts = new HashSet<int>(MivryQuestHands.getTrackingPointsIndices(this.left_hand_tracking_points));
            for (int part = this.leftHandPartsMin; part <= this.leftHandPartsMax; part++) {
                if (active_parts.Contains(part - this.leftHandPartsMin)) {
                    this.gc.setPartEnabled(part, true);
                    for (int gesture_id = this.gc.numberOfGestureCombinations() - 1; gesture_id >= 0; gesture_id--) {
                        MivryQuestHands.TrackedHand trackedHand = this.getTrackedHand(gesture_id);
                        if (trackedHand == MivryQuestHands.TrackedHand.LeftHand || trackedHand == MivryQuestHands.TrackedHand.BothHands) {
                            this.gc.setCombinationPartGesture(gesture_id, part, gesture_id);
                            this.gc.setGestureEnabled(part, gesture_id, true);
                        }
                    }
                } else {
                    this.gc.setPartEnabled(part, false);
                    for (int gesture_id = this.gc.numberOfGestureCombinations() - 1; gesture_id >= 0; gesture_id--) {
                        MivryQuestHands.TrackedHand trackedHand = this.getTrackedHand(gesture_id);
                        if (trackedHand == MivryQuestHands.TrackedHand.LeftHand || trackedHand == MivryQuestHands.TrackedHand.BothHands) {
                            this.gc.setCombinationPartGesture(gesture_id, part, -1);
                            this.gc.setGestureEnabled(part, gesture_id, false);
                        }
                    }
                }
            }
        }
    }
    [SerializeField] private MivryQuestHands.TrackingPoints right_hand_tracking_points = MivryQuestHands.TrackingPoints.AllFingerTips;
    public MivryQuestHands.TrackingPoints rightHandTrackingPoints
    {
        get
        {
            return this.right_hand_tracking_points;
        }
        set
        {
            this.right_hand_tracking_points = value;
            this.gc.setMetadataAsString($"MivryQuestHands LeftHandTrackingPoints={this.left_hand_tracking_points.ToString()} RightHandTrackingPoints={this.right_hand_tracking_points.ToString()}");
            HashSet<int> active_parts = new HashSet<int>(MivryQuestHands.getTrackingPointsIndices(this.right_hand_tracking_points));
            for (int part = this.rightHandPartsMin; part <= this.rightHandPartsMax; part++) {
                if (active_parts.Contains(part - this.rightHandPartsMin)) {
                    this.gc.setPartEnabled(part, true);
                    for (int gesture_id = this.gc.numberOfGestureCombinations() - 1; gesture_id >= 0; gesture_id--) {
                        MivryQuestHands.TrackedHand trackedHand = this.getTrackedHand(gesture_id);
                        if (trackedHand == MivryQuestHands.TrackedHand.LeftHand || trackedHand == MivryQuestHands.TrackedHand.BothHands) {
                            this.gc.setCombinationPartGesture(gesture_id, part, gesture_id);
                            this.gc.setGestureEnabled(part, gesture_id, true);
                        }
                    }
                } else {
                    this.gc.setPartEnabled(part, false);
                    for (int gesture_id = this.gc.numberOfGestureCombinations() - 1; gesture_id >= 0; gesture_id--) {
                        MivryQuestHands.TrackedHand trackedHand = this.getTrackedHand(gesture_id);
                        if (trackedHand == MivryQuestHands.TrackedHand.LeftHand || trackedHand == MivryQuestHands.TrackedHand.BothHands) {
                            this.gc.setCombinationPartGesture(gesture_id, part, -1);
                            this.gc.setGestureEnabled(part, gesture_id, false);
                        }
                    }
                }
            }
        }
    }

    public MivryQuestHands.GestureTrigger left_gesture_trigger = MivryQuestHands.GestureTrigger.RightHandGrab;
    public MivryQuestHands.GestureTrigger right_gesture_trigger = MivryQuestHands.GestureTrigger.LeftHandGrab;

    // Whether the user is currently holding the gesture trigger.
    private bool trigger_pressed_left = false;
    private bool trigger_pressed_right = false;
    private float trigger_value_left = 0;
    private float trigger_value_right = 0;

    [SerializeField] public float trigger_threshold = 0.8f;

    [System.NonSerialized] public OVRHand left_hand;
    [System.NonSerialized] public OVRSkeleton left_hand_skeleton;
    [System.NonSerialized] public OVRHand right_hand;
    [System.NonSerialized] public OVRSkeleton right_hand_skeleton;

    [System.NonSerialized] private Material inactive_pointer_material;
    [System.NonSerialized] private Material active_pointer_material;

    public MivryQuestHands.UnityXrPlugin unityXrPlugin = MivryQuestHands.UnityXrPlugin.OculusVR;
    public MivryQuestHands.MivryCoordinateSystem mivryCoordinateSystem = MivryQuestHands.MivryCoordinateSystem.OculusVR;

    [Serializable] public enum FrameOfReference
    {
        Head, Hand, World
    };
    public FrameOfReference frameOfReference = FrameOfReference.Head;

    [System.NonSerialized] public string[] file_imports = {
        "Samples/Sample_MivryQuestHands_Gestures.dat",
    };
    [System.NonSerialized] private bool file_importing_completed = false;

    public string file_load_gestures = "Samples/Sample_MivryQuestHands_Gestures.dat";
    public string file_import_gestures = "Samples/Sample_MivryQuestHands_Gestures.dat";
    public string file_save_gestures = "Samples/MyQuestHandsGestures.dat";

    public string create_gesture_name = "(new gesture name)";
    public string[] create_gesture_names = null;

    public int record_combination_id = -1;

    public int copy_gesture_from_part = 0; // combination part from where to copy a gesture
    public int copy_gesture_to_part = 1; // combination part to where to copy a gesture
    public int copy_gesture_from_id = 0; // which gesture to copy
    public int copy_gesture_to_id = 0; // which gesture to copy (+1 so that "0" means "new gesture (-1))
    public bool copy_gesture_mirror = true; // whether to mirror the gesture to copy
    public bool copy_gesture_rotate = true; // whether to rotate the gesture to copy

    // The gesture combinations object:
    [System.NonSerialized] public GestureCombinations gc = null;

    // The text field to display instructions.
    private Text ConsoleText;

    // Whether the training process is was recently started.
    [System.NonSerialized] public bool training_started = false;

    // Last reported recognition performance (during training).
    // 0 = 0% correctly recognized, 1 = 100% correctly recognized.
    [System.NonSerialized] public double last_performance_report = 0;

    // Whether the loading process is was recently started.
    [System.NonSerialized] public bool loading_started = false;

    // Progress of the loading process. A negative error code on failure.
    [System.NonSerialized] public int loading_progress = 0;

    // Result of the loading result. Zero on success, a negative error code on failure.
    [System.NonSerialized] public int loading_result = 0;

    // Progress of the saving process. A negative error code on failure.
    [System.NonSerialized] public int saving_progress = 0;

    // Whether the saving process is was recently started.
    [System.NonSerialized] public bool saving_started = false;

    // Result of the saving result. Zero on success, a negative error code on failure.
    [System.NonSerialized] public int saving_result = 0;

    // The path where the file was saved.
    private string saving_path = "";
    
    // Handle to this object/script instance, so that callbacks from the plug-in arrive at the correct instance.
    private GCHandle me;

    // Whether gesturing (performing gesture motions) is currently possible.
    [System.NonSerialized] public bool gesturing_enabled = true;

    // Wether a gesture was already started
    [System.NonSerialized] public bool gesture_started = false;

    // Whether or not to update (and thus compensate for) the frame-of-reference (head/hand) position during gesturing.
    public bool updateFrameOfReference
    {
        get
        {
            if (gc != null) {
                return gc.getUpdateHeadPositionPolicy(0) == GestureRecognition.UpdateHeadPositionPolicy.UseLatest;
            }
            return false;
        }
        set
        {
            GestureRecognition.UpdateHeadPositionPolicy p = value ? GestureRecognition.UpdateHeadPositionPolicy.UseLatest : GestureRecognition.UpdateHeadPositionPolicy.UseInitial;
            if (gc != null) {
                for (int part = gc.numberOfParts() - 1; part >= 0; part--) {
                    gc.setUpdateHeadPositionPolicy(part, p);
                }
            }
        }
    }

    // File/folder suggestions for the load files button
    [System.NonSerialized] public int file_suggestion = 0;
    [System.NonSerialized] public List<string> file_suggestions = new List<string>();

    public GestureManager() : base()
    {
        me = GCHandle.Alloc(this);
        this.gc = new GestureCombinations(this.numberOfParts);

        this.gc.setTrainingUpdateCallback(GestureManager.trainingUpdateCallback);
        this.gc.setTrainingUpdateCallbackMetadata((IntPtr)me);
        this.gc.setTrainingFinishCallback(GestureManager.trainingFinishCallback);
        this.gc.setTrainingFinishCallbackMetadata((IntPtr)me);
        this.gc.setLoadingUpdateCallbackFunction(GestureManager.loadingUpdateCallback);
        this.gc.setLoadingUpdateCallbackMetadata((IntPtr)me);
        this.gc.setLoadingFinishCallbackFunction(GestureManager.loadingFinishCallback);
        this.gc.setLoadingFinishCallbackMetadata((IntPtr)me);
        this.gc.setSavingUpdateCallbackFunction(GestureManager.savingUpdateCallback);
        this.gc.setSavingUpdateCallbackMetadata((IntPtr)me);
        this.gc.setSavingFinishCallbackFunction(GestureManager.savingFinishCallback);
        this.gc.setSavingFinishCallbackMetadata((IntPtr)me);

        // this.gc.setMetadataAsString($"MivryQuestHands LeftHandTrackingPoints={this.left_hand_tracking_points.ToString()} RightHandTrackingPoints={this.right_hand_tracking_points.ToString()}");
        this.leftHandTrackingPoints = this.left_hand_tracking_points; // sets the metadata and enabled parts
        this.rightHandTrackingPoints = this.right_hand_tracking_points; // sets the metadata and enabled parts
    }

    // Initialization:
    void Start ()
    {
        // Set the welcome message.
        ConsoleText = GameObject.Find("ConsoleText").GetComponent<Text>();
        consoleText = "Welcome to MiVRy Gesture Manager!\n"
                    + "(" + GestureRecognition.getVersionString() + ")\n"
                    + "This manager allows you to\ncreate and record gestures,\n"
                    + "and organize gesture files.";

        me = GCHandle.Alloc(this);

        this.gc.setMetadataAsString($"MivryQuestHands LeftHandTrackingPoints={this.left_hand_tracking_points.ToString()} RightHandTrackingPoints={this.right_hand_tracking_points.ToString()}");

        this.left_hand = GameObject.Find("LeftHandAnchor").transform.Find("OVRHandPrefab").GetComponent<OVRHand>();
        this.right_hand = GameObject.Find("RightHandAnchor").transform.Find("OVRHandPrefab").GetComponent<OVRHand>();
        this.left_hand_skeleton = this.left_hand.GetComponent<OVRSkeleton>();
        this.right_hand_skeleton = this.right_hand.GetComponent<OVRSkeleton>();
        this.inactive_pointer_material = (Material)Resources.Load("GesturemanagerInactivePointerMaterial", typeof(Material));
        this.active_pointer_material = (Material)Resources.Load("GesturemanagerActivePointerMaterial", typeof(Material));
    }


    // Update:
    public void Update()
    {
        int ret;

#if ENABLE_INPUT_SYSTEM
        if (Keyboard.current.escapeKey.wasPressedThisFrame) {
            Application.Quit();
        }
#else
        try {
            float escape = Input.GetAxis("escape");
            if (escape > 0.0f) {
                Application.Quit();
            }
        } catch (Exception) { }
#endif

        if (!this.file_importing_completed) {
            for (int i = this.file_imports.Length - 1; i >= 0; i--) {
                this.importFromStreamingAssets(this.file_imports[i]);
            }
            this.file_importing_completed = true;
        }

        if (this.gc == null) {
            consoleText = "Welcome to MiVRy Gesture Manager!\n"
                             + "(" + GestureRecognition.getVersionString() + ")\n"
                             + "This manager allows you to\ncreate and record gestures,\n"
                             + "and organize gesture files.";
            return;
        }

        if (!this.license_activated) {
            if (this.license_id != null && this.license_id.Length > 0) {
                if (this.license_key != null && this.license_key.Length > 0) {
                    ret = this.gc.activateLicense(this.license_id, this.license_key);
                    if (ret == 0) {
                        Debug.Log("License successfully activated");
                        this.license_activated = true;
                    } else {
                        Debug.LogError("Failed to activate license: " + GestureRecognition.getErrorMessage(ret));
                    }
                }
            } else if (this.license_file_path != null && this.license_file_path.Length > 0) {
                ret = this.gc.activateLicenseFile(this.license_file_path);
                if (ret == 0) {
                    Debug.Log("License successfully activated");
                    this.license_activated = true;
                } else {
                    Debug.LogError("Failed to activate license: " + GestureRecognition.getErrorMessage(ret));
                }
            }
        }

        if (training_started) {
            if (this.gc.isTraining()) {
                consoleText = "Currently training...\n"
                            + "Current recognition performance: " + (this.last_performance_report * 100).ToString("0.00") + "%.\n";
                GestureManagerVR.refresh();
                return;
            } else {
                training_started = false;
                consoleText = "Training finished!\n"
                            + "Final recognition performance: " + (this.last_performance_report * 100).ToString("0.00") + "%.\n";
                GestureManagerVR.refresh();
            }
        } else if (this.gc.isTraining()) {
            training_started = true;
            consoleText = "Currently training...\n"
                        + "Current recognition performance: " + (this.last_performance_report * 100).ToString("0.00") + "%.\n";
            GestureManagerVR.refresh();
            return;
        }

        if (loading_started) {
            if (this.gc.isLoading()) {
                consoleText = $"Currently loading...\n({this.loading_progress}%)";
                return;
            } else {
                loading_started = false;
                if (this.loading_result != 0) {
                    consoleText = "Loading failed!\n"
                                 + GestureRecognition.getErrorMessage(this.loading_result);
                    return;
                }
                string metadata = this.gc.getMetadataAsString();
                if (metadata == null) {
                    consoleText = "Loading finished,\nbut file was not a valid MivryQuestHands database.";
                    return;
                }
                string[] metadata_parts = metadata.Split(' ');
                if (metadata_parts.Length != 3 || metadata_parts[0] != "MivryQuestHands") {
                    consoleText = "Loading finished,\nbut file was not a valid MivryQuestHands database.";
                    return;
                }
                string[] metadata_lhtp = metadata_parts[1].Split('=');
                string[] metadata_rhtp = metadata_parts[2].Split('=');
                if (metadata_lhtp.Length != 2 || metadata_rhtp.Length != 2) {
                    consoleText = "Loading finished,\nbut file was not a valid MivryQuestHands database.";
                    return;
                }
                switch (metadata_lhtp[1]) {
                    case "AllBones":
                        this.left_hand_tracking_points = MivryQuestHands.TrackingPoints.AllBones;
                        break;
                    case "AllFingerTips":
                        this.left_hand_tracking_points = MivryQuestHands.TrackingPoints.AllFingerTips;
                        break;
                    case "IndexFingerTipOnly":
                        this.left_hand_tracking_points = MivryQuestHands.TrackingPoints.IndexFingerTipOnly;
                        break;
                    default:
                        consoleText = "Loading finished,\nbut file was not a valid MivryQuestHands database.";
                        return;
                }
                switch (metadata_rhtp[1]) {
                    case "AllBones":
                        this.right_hand_tracking_points = MivryQuestHands.TrackingPoints.AllBones;
                        break;
                    case "AllFingerTips":
                        this.right_hand_tracking_points = MivryQuestHands.TrackingPoints.AllFingerTips;
                        break;
                    case "IndexFingerTipOnly":
                        this.right_hand_tracking_points = MivryQuestHands.TrackingPoints.IndexFingerTipOnly;
                        break;
                    default:
                        consoleText = "Loading finished,\nbut file was not a valid MivryQuestHands database.";
                        return;
                }
                if (this.numberOfParts != this.gc.numberOfParts()) {
                    consoleText = "Loading finished,\nbut file was not a valid MivryQuestHands database.";
                    return;
                }
                GestureManagerVR.refresh();
                consoleText = "Loading finished successfully.";
            }
        } else if (this.gc.isLoading()) {
            loading_started = true;
            consoleText = "Currently loading...\n";
            GestureManagerVR.refresh();
            return;
        }

        if (saving_started) {
            if (this.gc.isSaving()) {
                consoleText = $"Currently saving...\n({this.saving_progress}%)";
                return;
            } else {
                saving_started = false;
                if (this.saving_result == 0) {
                    consoleText = "Saving Finished!\n"
                        + "File Location : " + this.saving_path;
                } else if (this.saving_result == -3) {
                    consoleText = $"Saving Failed!\nThe path {this.saving_path} is not writable.";
                } else {
                    consoleText = "Saving Failed!\n"
                        + GestureRecognition.getErrorMessage(this.saving_result);
                }
                GestureManagerVR.refresh();
            }
        } else if (this.gc.isSaving()) {
            saving_started = true;
            consoleText = "Currently saving...\n";
            GestureManagerVR.refresh();
            return;
        }

        if (this.left_hand == null || this.right_hand == null || !this.gesturing_enabled) {
            return;
        }

        if (this.left_gesture_trigger != MivryQuestHands.GestureTrigger.Manual) {
            this.trigger_value_left  = (0.75f * this.trigger_value_left ) + (0.25f * MivryQuestHands.getTriggerValue(this.left_gesture_trigger, this.left_hand, this.right_hand) );
        }
        if (this.right_gesture_trigger != MivryQuestHands.GestureTrigger.Manual) {
            this.trigger_value_right = (0.75f * this.trigger_value_right) + (0.25f * MivryQuestHands.getTriggerValue(this.right_gesture_trigger, this.left_hand, this.right_hand));
        }

        GameObject hmd = Camera.main.gameObject; // alternative: GameObject.Find("Main Camera");
        Vector3 hmd_p = hmd.transform.position;
        Quaternion hmd_q = hmd.transform.rotation;
        MivryQuestHands.convertHeadInput(this.mivryCoordinateSystem, ref hmd_p, ref hmd_q);

        // If the user presses either hand's trigger, we start a new gesture.
        if (trigger_pressed_left == false && trigger_value_left > this.trigger_threshold) {
            GestureManagerVR.me.pointerLeft.GetComponent<MeshRenderer>().material = this.active_pointer_material;
            // hand trigger pressed.
            trigger_pressed_left = true;
            this.startStroke(this.left_hand, this.leftHandTrackingPoints, this.leftHandPartsMin);
            gesture_started = true;
        }
        if (trigger_pressed_right == false && trigger_value_right > this.trigger_threshold) {
            GestureManagerVR.me.pointerRight.GetComponent<MeshRenderer>().material = this.active_pointer_material;
            // hand trigger pressed.
            trigger_pressed_right = true;
            this.startStroke(this.right_hand, this.rightHandTrackingPoints, this.rightHandPartsMin);
            gesture_started = true;
        }
        if (gesture_started == false) {
            // nothing to do.
            return;
        }

        // If we arrive here, the user is currently gesturing with one of the hands.
        if (this.updateFrameOfReference) {
            gc.updateHeadPosition(hmd_p, hmd_q);
        }
        if (trigger_pressed_left == true) {
            if (trigger_value_left < this.trigger_threshold * 0.9f) {
                // User let go of the trigger
                GestureManagerVR.me.pointerLeft.GetComponent<MeshRenderer>().material = this.inactive_pointer_material;
                this.endStroke(this.leftHandTrackingPoints, this.leftHandPartsMin);
                trigger_pressed_left = false;
            } else {
                // User still gesturing while trigger pressed
                this.contdStroke(this.left_hand_skeleton, this.leftHandTrackingPoints, this.leftHandPartsMin);
            }
        }

        if (trigger_pressed_right == true) {
            if (trigger_value_right < this.trigger_threshold * 0.9f) {
                // User let go of the trigger
                GestureManagerVR.me.pointerRight.GetComponent<MeshRenderer>().material = this.inactive_pointer_material;
                this.endStroke(this.rightHandTrackingPoints, this.rightHandPartsMin);
                trigger_pressed_right = false;
            } else {
                // User still gesturing while trigger pressed
                this.contdStroke(this.right_hand_skeleton, this.rightHandTrackingPoints, this.rightHandPartsMin);
            }
        }

        if (trigger_pressed_left || trigger_pressed_right) {
            // User still dragging with either hand - nothing left to do
            return;
        }
        // else: if we arrive here, the user let go of both triggers, ending the gesture.
        gesture_started = false;

        // If we are currently recording samples:
        if (this.record_combination_id >= 0) {
            // Currently recording samples - show how many we have recorded so far.
            int num_samples_left = gc.getGestureNumberOfSamples(this.leftHandPartsMin, this.record_combination_id);
            int num_samples_right = gc.getGestureNumberOfSamples(this.rightHandPartsMin, this.record_combination_id);
            MivryQuestHands.TrackedHand trackedHand = this.getTrackedHand(this.record_combination_id);
            string num_samples_text = (trackedHand == MivryQuestHands.TrackedHand.LeftHand)
                ? num_samples_left.ToString()
                : (trackedHand == MivryQuestHands.TrackedHand.RightHand)
                ? num_samples_right.ToString()
                : $"{num_samples_left} left / {num_samples_right} right";
            consoleText = "Recorded a gesture sample for " + gc.getGestureCombinationName(record_combination_id) + ".\n"
                        + "Total number of recorded samples for this gesture: " + num_samples_text;
            GestureManagerVR.refresh();
            return;
        }
        // else: if we arrive here, we're not recording new samples for custom gestures,
        // but instead try to identidy a gesture.
        double similarity = 0; // This will receive a similarity value (0~1).
        int recognized_combination_id = gc.identifyGestureCombination(ref similarity);
        // Perform the action associated with that gesture.
        if (recognized_combination_id < 0) {
            // Error trying to identify any gesture
            consoleText = "Failed to identify gesture: " + GestureRecognition.getErrorMessage(recognized_combination_id);
        } else {
            string combination_name = gc.getGestureCombinationName(recognized_combination_id);
            consoleText = "Identified gesture combination '"+ combination_name+"' ("+ recognized_combination_id + ")\n(Similarity: " + similarity.ToString("0.000") + ")";
        }
    }

    private void startStroke(OVRHand hand, MivryQuestHands.TrackingPoints trackingPoints, int partMin)
    {
        Vector3 p;
        Quaternion q;
        switch (this.frameOfReference) {
            case FrameOfReference.World:
                p = Vector3.zero;
                q = Quaternion.identity;
                break;
            case FrameOfReference.Hand:
                p = hand.transform.position;
                q = hand.transform.rotation;
                break;
            case FrameOfReference.Head:
            default:
                Transform hmd = Camera.main.gameObject.transform; // alternative: GameObject.Find("Main Camera");
                p = hmd.position;
                q = hmd.rotation;
                MivryQuestHands.convertHeadInput(this.mivryCoordinateSystem, ref p, ref q);
                break;
        }

        if (this.record_combination_id >= 0) {
            trackingPoints = MivryQuestHands.TrackingPoints.AllBones;
        }
        int ret;
        int boneIndex;

        switch (trackingPoints) {
            case MivryQuestHands.TrackingPoints.IndexFingerTipOnly:
                boneIndex = (int)OVRSkeleton.BoneId.Hand_IndexTip;
                ret = gc.startStroke(partMin + boneIndex, p, q, this.record_combination_id);
                if (ret != 0) {
                    Debug.LogError($"[GestureManager.startStroke] startStroke() failed with '{GestureRecognition.getErrorMessage(ret)}'");
                }
                break;
            case MivryQuestHands.TrackingPoints.AllFingerTips:
                for (int i = MivryQuestHands.fingerTipIndices.Length - 1; i >= 0; i--) {
                    boneIndex = MivryQuestHands.fingerTipIndices[i];
                    ret = this.gc.startStroke(partMin + boneIndex, p, q, this.record_combination_id);
                    if (ret != 0) {
                        Debug.LogError($"[GestureManager.startStroke] startStroke() failed with '{GestureRecognition.getErrorMessage(ret)}'");
                    }
                }
                break;
            case MivryQuestHands.TrackingPoints.AllBones:
                for (boneIndex = (int)OVRSkeleton.BoneId.Hand_End - 1; boneIndex >= 0; boneIndex--) {
                    ret = this.gc.startStroke(partMin + boneIndex, p, q, this.record_combination_id);
                    if (ret != 0) {
                        Debug.LogError($"[GestureManager.startStroke] startStroke() failed with '{GestureRecognition.getErrorMessage(ret)}'");
                    }
                }
                break;
        }
    }

    private void contdStroke(OVRSkeleton hand, MivryQuestHands.TrackingPoints trackingPoints, int partMin)
    {
        if (this.record_combination_id >= 0) {
            trackingPoints = MivryQuestHands.TrackingPoints.AllBones;
        }
        Transform t;
        Vector3 p;
        Quaternion q;
        int ret;
        int boneIndex;

        switch (this.frameOfReference) {
            case FrameOfReference.World:
                p = Vector3.zero;
                q = Quaternion.identity;
                break;
            case FrameOfReference.Hand:
                p = hand.transform.position;
                q = hand.transform.rotation;
                break;
            case FrameOfReference.Head:
            default:
                Transform hmd = Camera.main.gameObject.transform; // alternative: GameObject.Find("Main Camera");
                p = hmd.position;
                q = hmd.rotation;
                MivryQuestHands.convertHeadInput(this.mivryCoordinateSystem, ref p, ref q);
                break;
        }
        ret = gc.updateHeadPosition(p, q);
        if (ret != 0) {
            Debug.LogError($"[GestureManager.contdStroke] updateHeadPosition() failed with '{GestureRecognition.getErrorMessage(ret)}'");
        }

        switch (trackingPoints) {
            case MivryQuestHands.TrackingPoints.IndexFingerTipOnly:
                boneIndex = (int)OVRSkeleton.BoneId.Hand_IndexTip;
                t = hand.Bones[boneIndex].Transform;
                p = t.position;
                q = t.rotation;
                MivryQuestHands.convertHandInput(this.unityXrPlugin, this.mivryCoordinateSystem, ref p, ref q);
                ret = gc.contdStrokeQ(partMin + boneIndex, p, q);
                if (ret != 0) {
                    Debug.LogError($"[GestureManager.contdStroke] contdStrokeQ() failed with '{GestureRecognition.getErrorMessage(ret)}'");
                }
                break;
            case MivryQuestHands.TrackingPoints.AllFingerTips:
                for (int i = MivryQuestHands.fingerTipIndices.Length-1; i >= 0; i--) {
                    boneIndex = MivryQuestHands.fingerTipIndices[i];
                    t = hand.Bones[boneIndex].Transform;
                    p = t.position;
                    q = t.rotation;
                    MivryQuestHands.convertHandInput(this.unityXrPlugin, this.mivryCoordinateSystem, ref p, ref q);
                    ret = this.gc.contdStrokeQ(partMin + boneIndex, p, q);
                    if (ret != 0) {
                        Debug.LogError($"[GestureManager.contdStroke] contdStrokeQ() failed with '{GestureRecognition.getErrorMessage(ret)}'");
                    }
                }
                break;
            case MivryQuestHands.TrackingPoints.AllBones:
                for (boneIndex = (int)OVRSkeleton.BoneId.Hand_End - 1; boneIndex >= 0; boneIndex--) {
                    t = hand.Bones[boneIndex].Transform;
                    p = t.position;
                    q = t.rotation;
                    MivryQuestHands.convertHandInput(this.unityXrPlugin, this.mivryCoordinateSystem, ref p, ref q);
                    ret = this.gc.contdStrokeQ(partMin + boneIndex, p, q);
                    if (ret != 0) {
                        Debug.LogError($"[GestureManager.contdStroke] contdStrokeQ() failed with '{GestureRecognition.getErrorMessage(ret)}'");
                    }
                }
                break;
        }
    }

    private void endStroke(MivryQuestHands.TrackingPoints trackingPoints, int partMin)
    {
        if (this.record_combination_id >= 0) {
            trackingPoints = MivryQuestHands.TrackingPoints.AllBones;
        }
        int ret;
        int boneIndex;

        switch (trackingPoints) {
            case MivryQuestHands.TrackingPoints.IndexFingerTipOnly:
                boneIndex = (int)OVRSkeleton.BoneId.Hand_IndexTip;
                ret = gc.endStroke(partMin + boneIndex);
                if (ret != 0) {
                    Debug.LogError($"[GestureManager.endStroke] endStroke({partMin + boneIndex}) failed with {GestureRecognition.getErrorMessage(ret)}");
                }
                break;
            case MivryQuestHands.TrackingPoints.AllFingerTips:
                for (int i = MivryQuestHands.fingerTipIndices.Length - 1; i >= 0; i--) {
                    boneIndex = MivryQuestHands.fingerTipIndices[i];
                    ret = this.gc.endStroke(partMin + boneIndex);
                    if (ret != 0) {
                        Debug.LogError($"[GestureManager.endStroke] endStroke({partMin + boneIndex}) failed with {GestureRecognition.getErrorMessage(ret)}");
                    }
                }
                break;
            case MivryQuestHands.TrackingPoints.AllBones:
                for (boneIndex = (int)OVRSkeleton.BoneId.Hand_End - 1; boneIndex >= 0; boneIndex--) {
                    ret = this.gc.endStroke(partMin + boneIndex);
                    if (ret != 0) {
                        Debug.LogError($"[GestureManager.endStroke] endStroke({partMin + boneIndex}) failed with {GestureRecognition.getErrorMessage(ret)}");
                    }
                }
                break;
        }
    }

    // Callback function to be called by the gesture recognition plug-in during the learning process.
    [MonoPInvokeCallback(typeof(GestureRecognition.TrainingCallbackFunction))]
    public static void trainingUpdateCallback(double performance, IntPtr ptr)
    {
        if (ptr.ToInt32() == 0) {
            return;
        }
        // Get the script/scene object back from metadata.
        GCHandle obj = (GCHandle)ptr;
        GestureManager me = (obj.Target as GestureManager);
        // Update the performance indicator with the latest estimate.
        me.last_performance_report = performance;
    }

    // Callback function to be called by the gesture recognition plug-in when the learning process was finished.
    [MonoPInvokeCallback(typeof(GestureRecognition.TrainingCallbackFunction))]
    public static void trainingFinishCallback(double performance, IntPtr ptr)
    {
        if (ptr.ToInt32() == 0) {
            return;
        }
        // Get the script/scene object back from metadata.
        GCHandle obj = (GCHandle)ptr;
        GestureManager me = (obj.Target as GestureManager);
        // Update the performance indicator with the latest estimate.
        me.last_performance_report = performance;
    }

    // Callback function to be called by the gesture recognition plug-in during the loading process.
    [MonoPInvokeCallback(typeof(GestureRecognition.LoadingCallbackFunction))]
    public static void loadingUpdateCallback(int progress, IntPtr ptr)
    {
        if (ptr.ToInt32() == 0) {
            return;
        }
        // Get the script/scene object back from metadata.
        GCHandle obj = (GCHandle)ptr;
        GestureManager me = (obj.Target as GestureManager);
        me.loading_progress = progress;
    }

    // Callback function to be called by the gesture recognition plug-in when the loading process was finished.
    [MonoPInvokeCallback(typeof(GestureRecognition.LoadingCallbackFunction))]
    public static void loadingFinishCallback(int result, IntPtr ptr)
    {
        if (ptr.ToInt32() == 0) {
            return;
        }
        // Get the script/scene object back from metadata.
        GCHandle obj = (GCHandle)ptr;
        GestureManager me = (obj.Target as GestureManager);
        me.loading_result = result;
    }

    // Callback function to be called by the gesture recognition plug-in during the saving process.
    [MonoPInvokeCallback(typeof(GestureRecognition.SavingCallbackFunction))]
    public static void savingUpdateCallback(int progress, IntPtr ptr)
    {
        if (ptr.ToInt32() == 0) {
            return;
        }
        // Get the script/scene object back from metadata.
        GCHandle obj = (GCHandle)ptr;
        GestureManager me = (obj.Target as GestureManager);
        me.saving_progress = progress;
    }

    // Callback function to be called by the gesture recognition plug-in when the saving process was finished.
    [MonoPInvokeCallback(typeof(GestureRecognition.SavingCallbackFunction))]
    public static void savingFinishCallback(int result, IntPtr ptr)
    {
        if (ptr.ToInt32() == 0) {
            return;
        }
        // Get the script/scene object back from metadata.
        GCHandle obj = (GCHandle)ptr;
        GestureManager me = (obj.Target as GestureManager);
        me.saving_result = result;
    }

    public bool importFromStreamingAssets(string file)
    {
        string srcPath = Application.streamingAssetsPath + "/" + file;
        string dstPath = Application.persistentDataPath + "/" + file;
#if !UNITY_EDITOR && UNITY_ANDROID
        // On android, the file is in the .apk,
        // so we need to first "download" it to the apps' cache folder.
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        UnityWebRequest request = UnityWebRequest.Get(srcPath);
        request.SendWebRequest();
        while (!request.isDone) {
            // wait for file extraction to finish
        }
        if (request.result == UnityWebRequest.Result.ConnectionError) {
            this.consoleText = "Failed to extract sample gesture database file from apk.";
            return false;
        }
        // string cachePath = activity.Call<AndroidJavaObject>("getCacheDir").Call<string>("getCanonicalPath");
        try {
            Directory.CreateDirectory(dstPath);
        } catch (Exception /* e */) { }
        try {
            Directory.Delete(dstPath);
        } catch (Exception /* e */) { }
        try {
            File.WriteAllBytes(dstPath, request.downloadHandler.data);
        } catch (Exception e) {
            this.consoleText = "Exception writing temporary file: " + e.ToString();
            return false;
        }
        return true;
#else
        try {
            Directory.CreateDirectory(dstPath);
        } catch (Exception /* e */) { }
        try {
            Directory.Delete(dstPath);
        } catch (Exception /* e */) { }
        try {
            File.Copy(srcPath, dstPath, true);
        } catch (Exception e) {
            this.consoleText = "Exception importing file: " + e.ToString();
            return false;
        }
        return true;
#endif
    }

    // Helper function to get the actual file path for a file to load
    public string getLoadPath(string file)
    {
        return (Path.IsPathRooted(file))
            ? file
            : Application.persistentDataPath + "/" + file;
    }

    // Helper function to get the actual file path for a file to save
    public string getSavePath(string file)
    {
        return (Path.IsPathRooted(file))
            ? file
            : Application.persistentDataPath + "/" + file;
    }

    // Helper function to load from gestures file.
    public bool loadFromFile()
    {
        if (this.gc == null) {
            this.consoleText = "[ERROR] No Gesture Recognition object\nto load gestures into.";
            return false;
        }
        string path = getLoadPath(this.file_load_gestures);
        this.consoleText = "Loading Gesture Combinations file...";
        int ret = this.gc.loadFromFileAsync(path);
        if (ret != 0) {
            byte[] file_contents = File.ReadAllBytes(path);
            if (file_contents == null || file_contents.Length == 0)
            {
                this.consoleText = $"Could not find gesture database file\n({path}).";
                return false;
            }
            ret = this.gc.loadFromBuffer(file_contents);
            if (ret != 0)
            {
                this.consoleText = $"[ERROR] Failed to load gesture combinations file\n{path}\n{GestureRecognition.getErrorMessage(ret)}";
                return false;
            }
        }
        this.loading_started = true;
        return true;
    }

    // Helper function to import gestures file.
    public bool importFromFile()
    {
        if (this.gc == null) {
            this.consoleText = "[ERROR] No Gesture Recognition object\nto import gestures into.";
            return false;
        }
        string path = getLoadPath(this.file_import_gestures);
        int ret = this.gc.importFromFile(path);
        if (ret == 0) {
            this.consoleText = "Gesture combinations file imported successfully";
            return true;
        } else {
            this.consoleText = $"[ERROR] Failed to import gesture combinations file\n{path}\n{GestureRecognition.getErrorMessage(ret)}";
            return false;
        }
    }

    // Helper function to save gestures to file. 
    public bool saveToFile()
    {
        if (this.gc == null) {
            this.consoleText = "[ERROR] No Gesture Recognition object\nto save to file.";
            return false;
        }
        this.saving_path = getSavePath(this.file_save_gestures);
        int ret = this.gc.saveToFileAsync(this.saving_path);
        if (ret == 0) {
            this.consoleText = "Started saving file at\n" + this.saving_path;
            this.saving_started = true;
            return true;
        } else {
            this.consoleText = $"[ERROR] Failed to save gesture combinations file at \n'{this.saving_path}'\n{GestureRecognition.getErrorMessage(ret)}";
            return false;
        }
    }

    public string consoleText
    {
        set
        {
            if (this.ConsoleText != null)
                this.ConsoleText.text = value;
            else
                Debug.Log(value);
        }
    }

    public int createGesture(string name)
    {
        int combo_id = this.gc.createGestureCombination(name);
        if (combo_id < 0) {
            consoleText = $"Failed to create gesture: '{GestureRecognition.getErrorMessage(combo_id)}'";
            return combo_id;
        }
        string metadata = $"MivryQuestHands TrackedHand={MivryQuestHands.TrackedHand.BothHands.ToString()}";
        if (this.gc.setGestureCombinationMetadataAsString(combo_id, metadata) != 0) {
            Debug.LogError($"[MivryQuestHands.GestureManager.createGesture] Failed to set metadata for new gesture {name} ({combo_id})");
        }
        var leftHandActiveIndices = new HashSet<int>(MivryQuestHands.getTrackingPointsIndices(this.leftHandTrackingPoints));
        for (int part = this.leftHandPartsMin; part <= this.leftHandPartsMax; part++) {
            int gid = this.gc.createGesture(part, $"New Gesture [{part}]");
            if (gid != combo_id) {
                consoleText = $"Failed to create sub-gesture part {part}: '{GestureRecognition.getErrorMessage(gid)}' ({gid})";
            } else {
                // this.gc.setGestureMetadataAsString(part, gid, metadata);
                if (false == leftHandActiveIndices.Contains(part - leftHandPartsMin)) { // currently not used, but kept to collect data
                    this.gc.setGestureEnabled(part, gid, false);
                    gid = -1;
                }
                int ret = this.gc.setCombinationPartGesture(combo_id, part, gid);
                if (ret < 0) {
                    consoleText = $"Failed to initiate sub-gesture part {part}: '{GestureRecognition.getErrorMessage(gid)}'";
                }
            }
        }
        var rightHandActiveIndices = new HashSet<int>(MivryQuestHands.getTrackingPointsIndices(this.rightHandTrackingPoints));
        for (int part = this.rightHandPartsMin; part <= this.rightHandPartsMax; part++) {
            int gid = this.gc.createGesture(part, $"New Gesture [{part}]");
            if (gid != combo_id) {
                consoleText = $"Failed to create sub-gesture part {part}: '{GestureRecognition.getErrorMessage(gid)}' ({gid})";
            } else {
                // this.gc.setGestureMetadataAsString(part, gid, metadata);
                if (false == rightHandActiveIndices.Contains(part - rightHandPartsMin)) { // currently not used, but kept to collect data
                    this.gc.setGestureEnabled(part, gid, false);
                    gid = -1;
                }
                int ret = this.gc.setCombinationPartGesture(combo_id, part, gid);
                if (ret < 0) {
                    consoleText = $"Failed to initiate sub-gesture part {part}: '{GestureRecognition.getErrorMessage(gid)}'";
                }
            }
        }
        return combo_id;
    }

    public bool deleteGesture(int combo_id)
    {
        if (combo_id < 0 || this.gc==null || combo_id >= this.gc.numberOfGestureCombinations()) {
            return false;
        }
        int ret = this.gc.deleteGestureCombination(combo_id);
        if (ret < 0) {
            consoleText = $"Failed to delete gesture: '{GestureRecognition.getErrorMessage(ret)}'";
            return false;
        }
        for (int part=this.gc.numberOfParts()-1; part >=0; part--) {
            ret = this.gc.deleteGesture(part, combo_id);
            if (ret < 0) {
                consoleText = $"Failed to delete sub-gesture part {part}: '{GestureRecognition.getErrorMessage(ret)}'";
            }
        }
        for (int cid = this.gc.numberOfGestureCombinations()-1; cid >= combo_id; cid--) {
            for (int part = this.gc.numberOfParts() - 1; part >= 0; part--) {
                this.gc.setCombinationPartGesture(cid, part, cid);
            }
        }
        return true;
    }
}
