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
using UnityEngine.UI;

public class Sample_MivryQuestHands : MonoBehaviour
{
    public Text ConsoleText;

    void Start()
    {
        
    }

    void Update()
    {
        Vector3 v = Camera.main.transform.worldToLocalMatrix.MultiplyPoint3x4(this.transform.position);
        if (Mathf.Abs(v.x) > 0.5f || Mathf.Abs(v.y) > 0.3f || v.z < 0.6f || v.z > 1.0f) {
            v = new Vector3(
                Mathf.Clamp(v.x, -0.5f, 0.5f),
                Mathf.Clamp(v.y, -0.3f, 0.3f),
                Mathf.Clamp(v.z,  0.6f, 1.0f)
            );
            v = Camera.main.transform.localToWorldMatrix.MultiplyPoint3x4(v);
            this.transform.position = 0.9f * this.transform.position + 0.1f * v;
        }
        Vector3 lookDir = Camera.main.transform.position - this.transform.position;
        lookDir.y = 0; // not facing up or down
        Quaternion q = Quaternion.LookRotation(lookDir) * Quaternion.AngleAxis(180.0f, Vector3.up);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, q, 0.1f);
    }

    public void OnGestureCompleted(GestureCompletionData data)
    {
        string text;
        if (data.gestureID >= 0) {
            text = $"Identified gesture:\n{data.gestureName}\n(Similarity: {data.similarity * 100.0f}%)";
        } else {
            text = $"Failed to identify gesture.\n{GestureRecognition.getErrorMessage(data.gestureID)}";
        }
        if (ConsoleText != null) {
            ConsoleText.text = text;
        }
        Debug.Log(text);
    }
}
