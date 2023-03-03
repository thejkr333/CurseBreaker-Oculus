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

#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(GestureManager))]
public class GestureManagerEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        // DrawDefaultInspector();

        GestureManager gm = (GestureManager)target;
        gm.Update();

        serializedObject.Update();
        var tracked_hand_prop = serializedObject.FindProperty("tracked_hand");
        var left_hand_tracking_points_prop = serializedObject.FindProperty("left_hand_tracking_points");
        var right_hand_tracking_points_prop = serializedObject.FindProperty("right_hand_tracking_points");
        var license_id_prop = serializedObject.FindProperty("license_id");
        var license_key_prop = serializedObject.FindProperty("license_key");
        var license_file_path_prop = serializedObject.FindProperty("license_file_path");
        var unityXrPlugin_prop = serializedObject.FindProperty("unityXrPlugin");
        var mivryCoordinateSystem_prop = serializedObject.FindProperty("mivryCoordinateSystem");
        var frameOfReference_prop = serializedObject.FindProperty("frameOfReference");
        var file_load_gestures_prop = serializedObject.FindProperty("file_load_gestures");
        var file_import_gestures_prop = serializedObject.FindProperty("file_import_gestures");
        var file_save_gestures_prop = serializedObject.FindProperty("file_save_gestures");
        var create_gesture_name_prop = serializedObject.FindProperty("create_gesture_name");
        // var create_gesture_names_prop = serializedObject.FindProperty("create_gesture_names");
        var record_combination_id_prop = serializedObject.FindProperty("record_combination_id");
        var left_gesture_trigger_prop = serializedObject.FindProperty("left_gesture_trigger");
        var right_gesture_trigger_prop = serializedObject.FindProperty("right_gesture_trigger");
        var copy_gesture_from_part_prop = serializedObject.FindProperty("copy_gesture_from_part");
        var copy_gesture_to_part_prop = serializedObject.FindProperty("copy_gesture_to_part");
        var copy_gesture_to_id_prop = serializedObject.FindProperty("copy_gesture_to_id");
        var copy_gesture_mirror_prop = serializedObject.FindProperty("copy_gesture_mirror");
        var copy_gesture_rotate_prop = serializedObject.FindProperty("copy_gesture_rotate");

        string[] tracking_points_enum = { "All bones", "All fingertips", "Index fingertip only" };
        EditorGUILayout.BeginVertical(GUI.skin.box);
        gm.leftHandTrackingPoints = (MivryQuestHands.TrackingPoints)(left_hand_tracking_points_prop.intValue = EditorGUILayout.Popup("Left hand tracking points", left_hand_tracking_points_prop.intValue, tracking_points_enum));
        gm.rightHandTrackingPoints = (MivryQuestHands.TrackingPoints)(right_hand_tracking_points_prop.intValue = EditorGUILayout.Popup("Right hand tracking points", right_hand_tracking_points_prop.intValue, tracking_points_enum));
        EditorGUILayout.EndVertical();

        string[] unityXrPlugins = { "OpenXR", "OculusVR", "SteamVR" };
        string[] mivryCoordinateSystems = { "OpenXR", "OculusVR", "SteamVR", "UnrealEngine" };

        if (gm.gc == null)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.LabelField("LICENSE:", "(Leave empty for free version)");
            license_id_prop.stringValue = gm.license_id = EditorGUILayout.TextField("Licence ID", license_id_prop.stringValue);
            license_key_prop.stringValue = gm.license_key = EditorGUILayout.TextField("Licence Key", license_key_prop.stringValue);
            license_file_path_prop.stringValue = gm.license_file_path = EditorGUILayout.TextField("Licence File Path", license_file_path_prop.stringValue);
            EditorGUILayout.EndVertical();
            if (GUI.changed)
            {
                EditorUtility.SetDirty(gm);
                EditorSceneManager.MarkSceneDirty(gm.gameObject.scene);
            }
            serializedObject.ApplyModifiedProperties();
            return;
        }

        // Gesture file management
        EditorGUILayout.Space();
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.LabelField("GESTURE FILES:");
        file_load_gestures_prop.stringValue = EditorGUILayout.TextField("Load gestures file:", file_load_gestures_prop.stringValue);
        if (GUILayout.Button("Load Gestures from File")) {
            gm.loadFromFile();
        }
        file_import_gestures_prop.stringValue = EditorGUILayout.TextField("Import gestures file:", file_import_gestures_prop.stringValue);
        if (GUILayout.Button("Import Gestures from File")) {
            gm.importFromFile();
        }
        file_save_gestures_prop.stringValue = EditorGUILayout.TextField("Save gestures file:", file_save_gestures_prop.stringValue);
        if (GUILayout.Button("Save Gestures to File")) {
            gm.saveToFile();
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space();
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.LabelField("GESTURES:");

        int num_gestures = gm.gc.numberOfGestureCombinations();
        string[] tracked_hand_enum = { "Left hand only", "Right hand only", "Both hands" };
        for (int i = 0; i < num_gestures; i++) {
            string gesture_name = gm.gc.getGestureCombinationName(i);
            GUILayout.BeginHorizontal();
            string new_gesture_name = EditorGUILayout.TextField(gesture_name);
            if (gesture_name != new_gesture_name) {
                int ret = gm.gc.setGestureCombinationName(i, new_gesture_name);
                if (ret == 0) {
                    for (int part = gm.gc.numberOfParts()-1; part>=0; part--) {
                        gm.gc.setGestureName(part, i, $"{new_gesture_name} [{part}]");
                    }
                } else {
                    Debug.Log("[ERROR] Failed to rename gesture: " + GestureRecognition.getErrorMessage(ret));
                }
            }
            MivryQuestHands.TrackedHand trackedHand = gm.getTrackedHand(i);
            gm.setTrackedHand(i, (MivryQuestHands.TrackedHand)EditorGUILayout.Popup((int)trackedHand, tracked_hand_enum));

            int gesture_samples_left = gm.gc.getGestureNumberOfSamples(gm.leftHandPartsMin, i);
            int gesture_samples_right = gm.gc.getGestureNumberOfSamples(gm.rightHandPartsMin, i);
            if (trackedHand == MivryQuestHands.TrackedHand.LeftHand) {
                EditorGUILayout.LabelField($"{gesture_samples_left} samples", GUILayout.Width(70));
            } else if (trackedHand == MivryQuestHands.TrackedHand.RightHand) {
                EditorGUILayout.LabelField($"{gesture_samples_right} samples", GUILayout.Width(70));
            } else if(trackedHand == MivryQuestHands.TrackedHand.BothHands) {
                EditorGUILayout.LabelField($"{gesture_samples_left}/{gesture_samples_right} samples", GUILayout.Width(70));
            } else {
                EditorGUILayout.LabelField($"??? samples", GUILayout.Width(70));
            }
            if (GUILayout.Button("Delete Last Sample")) {
                if (trackedHand == MivryQuestHands.TrackedHand.LeftHand || trackedHand == MivryQuestHands.TrackedHand.BothHands) {
                    for (int part = gm.leftHandPartsMax; part >= gm.leftHandPartsMin; part--) {
                        int ret = gm.gc.deleteGestureSample(part, i, gesture_samples_left - 1);
                        if (ret != 0) {
                            Debug.Log("[ERROR] Failed to delete gesture sample: " + GestureRecognition.getErrorMessage(ret));
                        }
                    }
                }
                if (trackedHand == MivryQuestHands.TrackedHand.RightHand || trackedHand == MivryQuestHands.TrackedHand.BothHands) {
                    for (int part = gm.rightHandPartsMax; part >= gm.rightHandPartsMin; part--) {
                        int ret = gm.gc.deleteGestureSample(part, i, gesture_samples_right - 1);
                        if (ret != 0) {
                            Debug.Log("[ERROR] Failed to delete gesture sample: " + GestureRecognition.getErrorMessage(ret));
                        }
                    }
                }
            }
            if (GUILayout.Button("Delete All Samples")) {
                for (int part = gm.gc.numberOfParts() - 1; part >= 0; part--) {
                    int ret = gm.gc.deleteAllGestureSamples(part, i);
                    if (ret != 0) {
                        Debug.Log("[ERROR] Failed to delete gesture samples: " + GestureRecognition.getErrorMessage(ret));
                    }
                }
            }
            if (GUILayout.Button("Delete Gesture")) {
                if (!gm.deleteGesture(i)) {
                    Debug.Log("[ERROR] Failed to delete gesture.");
                }
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.BeginHorizontal();
        create_gesture_name_prop.stringValue = EditorGUILayout.TextField(create_gesture_name_prop.stringValue);
        if (GUILayout.Button("Create new gesture")) {
            int gesture_id = gm.createGesture(gm.create_gesture_name);
            if (gesture_id < 0) {
                Debug.Log("[ERROR] Failed to create gesture.");
            } else {
                gm.create_gesture_name = "(new gesture name)";
            }
        }
        GUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
        
        EditorGUILayout.Space();
        EditorGUILayout.BeginVertical(GUI.skin.box);
        string[] framesOfReference = {"Head", "Hand", "World"};
        frameOfReference_prop.intValue = EditorGUILayout.Popup("Frame of Reference", frameOfReference_prop.intValue, framesOfReference);
        gm.updateFrameOfReference = EditorGUILayout.Toggle("Compensate head motion during gesture", gm.updateFrameOfReference);
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space();
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.LabelField("RECORD GESTURE SAMPLES:");

        string[] trigger_names = new string[] {
            "LeftHandPinch",
            "LeftHandGrab",
            "RightHandPinch",
            "RightHandGrab",
            "Manual",
        };
        left_gesture_trigger_prop.intValue = EditorGUILayout.Popup(new GUIContent(
            "Left hand gesture trigger",
            "How to indicate a start/end of a gesture motion of the left hand"),
            left_gesture_trigger_prop.intValue,
            trigger_names
        );
        right_gesture_trigger_prop.intValue = EditorGUILayout.Popup(new GUIContent(
            "Right hand gesture trigger",
            "How to indicate a start/end of a gesture motion of the right hand"),
            right_gesture_trigger_prop.intValue,
            trigger_names
        );

        int num_combinations = gm.gc.numberOfGestureCombinations();
        string[] combination_names = new string[num_combinations + 1];
        combination_names[0] = "[Testing, not recording]";
        for (int i = 0; i < num_combinations; i++)
        {
            combination_names[i+1] = gm.gc.getGestureCombinationName(i);
        }
        record_combination_id_prop.intValue = EditorGUILayout.Popup(record_combination_id_prop.intValue + 1, combination_names) - 1;
        EditorGUILayout.LabelField("COORDINATE SYSTEM CONVERSION:", "");
        unityXrPlugin_prop.intValue = EditorGUILayout.Popup("Unity XR Plug-in", unityXrPlugin_prop.intValue, unityXrPlugins);
        mivryCoordinateSystem_prop.intValue = EditorGUILayout.Popup("MiVRy Coordinate System", mivryCoordinateSystem_prop.intValue, mivryCoordinateSystems);
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space();
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.LabelField("START/STOP TRAINING:");
        
        if (gm.gc.isTraining()) {
            EditorGUILayout.LabelField("Performance:", (gm.last_performance_report * 100.0).ToString("0.00") + "%");
        } else {
            EditorGUILayout.LabelField("Performance:", (gm.gc.recognitionScore() * 100.0).ToString("0.00") + "%");
        }
        EditorGUILayout.LabelField("Currently training:", (gm.gc.isTraining() ? "yes" : "no"));
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Start training"))
        {
            if (gm.gc.isTraining())
            {
                Debug.Log("Already training...");
            } else {
                int ret = gm.gc.startTraining();
                if (ret == 0)
                {
                    gm.training_started = true;
                    Debug.Log("Training started");
                }
                else
                {
                    gm.training_started = false;
                    Debug.Log("[ERROR] Failed to start training: " + GestureRecognition.getErrorMessage(ret));
                }
            }
        }
        if (GUILayout.Button("Stop training"))
        {
            gm.gc.stopTraining();
        }
        GUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.LabelField("LICENSE:", "(Leave empty for free version)");
        license_id_prop.stringValue = gm.license_id = EditorGUILayout.TextField("Licence ID", license_id_prop.stringValue);
        license_key_prop.stringValue = gm.license_key = EditorGUILayout.TextField("Licence Key", license_key_prop.stringValue);
        license_file_path_prop.stringValue = gm.license_file_path = EditorGUILayout.TextField("Licence File Path", license_file_path_prop.stringValue);
        EditorGUILayout.EndVertical();
        if (GUI.changed)
        {
            EditorUtility.SetDirty(gm);
            EditorSceneManager.MarkSceneDirty(gm.gameObject.scene);
        }
        serializedObject.ApplyModifiedProperties();
    }
}
#endif
