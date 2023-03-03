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
using System.Text.RegularExpressions;

public class EditableTextField : MonoBehaviour
{
    [System.Serializable]
    public enum Target
    {
        GestureName,
        CombinationName,
        LoadFile,
        SaveFile
    }
    public Target target;

    public TextMesh displayText;

    public int maxDisplayLength;

    // Start is called before the first frame update
    void Start()
    {
        this.refreshText();
    }

    public void refreshText()
    {
        GestureManager gm = GestureManagerVR.me?.gestureManager;
        if (gm == null)
            return;
        if (displayText == null)
            return;

        string text = null;
        switch (this.target)
        {
            case Target.GestureName: {
                SubmenuGesture submenuGesture = this.transform.parent.gameObject.GetComponent<SubmenuGesture>();
                if (submenuGesture.CurrentGesture < 0)
                {
                    text = "";
                } else if (gm.gc != null)
                {
                    text = gm.gc.getGestureCombinationName(submenuGesture.CurrentGesture);
                } else
                {
                    text = "???";
                }
                } break;
            case Target.CombinationName: /*{
                SubmenuCombination submenuCombination = this.transform.parent.gameObject.GetComponent<SubmenuCombination>();
                if (submenuCombination.CurrentCombination < 0)
                {
                    text = "";
                }
                else if (gm.gc != null)
                {
                    text = gm.gc.getGestureCombinationName(submenuCombination.CurrentCombination);
                }
                else
                {
                    text = "???";
                }
                }*/ break;
            case Target.LoadFile:
                text = gm.file_load_gestures;
                break;
            case Target.SaveFile:
                text = gm.file_save_gestures;
                break;
            default:
                text = "???";
                break;
        }
        if (text.Length > this.maxDisplayLength)
            text = text.Substring(text.Length - this.maxDisplayLength);
        this.displayText.text = text;
    }

    public void setValue(string text)
    {
        GestureManager gm = GestureManagerVR.me?.gestureManager;
        if (gm == null)
            return;
        switch (this.target)
        {
            case Target.GestureName: {
                SubmenuGesture submenuGesture = this.transform.parent.gameObject.GetComponent<SubmenuGesture>();
                if (submenuGesture.CurrentGesture < 0) {
                    return;
                }
                if (gm.gc != null) {
                    gm.gc.setGestureCombinationName(submenuGesture.CurrentGesture, text);
                    for (int part=gm.gc.numberOfParts()-1; part>=0; part--) { 
                        gm.gc.setGestureName(part, submenuGesture.CurrentGesture, $"{text} [{part}]");
                    }
                }
                } break;
            case Target.CombinationName: /*{
                SubmenuCombination submenuCombination = this.transform.parent.gameObject.GetComponent<SubmenuCombination>();
                if (submenuCombination.CurrentCombination < 0)
                {
                    return;
                }
                gm.gc.setGestureCombinationName(submenuCombination.CurrentCombination, text);
                // update the name for all combination parts
                for (int part=gm.gc.numberOfParts()-1; part>=0; part--) {
                    gm.gc.setGestureName(part, submenuCombination.CurrentCombination, $"{text} [{part}]");
                }
                }*/ break;
            case Target.LoadFile:
                gm.file_load_gestures = text;
                break;
            case Target.SaveFile:
                gm.file_save_gestures = text;
                break;
        }
        this.refreshText();
    }

    public string getValue()
    {
        GestureManager gm = GestureManagerVR.me?.gestureManager;
        if (gm == null)
            return "";
        switch (this.target)
        {
            case Target.GestureName: {
                SubmenuGesture submenuGesture = this.transform.parent.gameObject.GetComponent<SubmenuGesture>();
                if (submenuGesture.CurrentGesture < 0)
                {
                    return "";
                }
                if (gm.gc != null)
                    return gm.gc.getGestureCombinationName(submenuGesture.CurrentGesture);
                else
                    return "[ERROR]";
                }
            case Target.CombinationName: /*{
                SubmenuCombination submenuCombination = this.transform.parent.gameObject.GetComponent<SubmenuCombination>();
                if (submenuCombination.CurrentCombination < 0)
                {
                    return "";
                }
                return gm.gc.getGestureCombinationName(submenuCombination.CurrentCombination);
                }*/ break;
            case Target.LoadFile:
                return gm.file_load_gestures;
            case Target.SaveFile:
                 return gm.file_save_gestures;
        }
        return "[ERROR]";
    }

    public void keyboardInput(KeyboardKey key)
    {
        if (displayText == null)
            return;
        this.setValue(key.applyTo(this.getValue()));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.name.EndsWith("pointer"))
            return;
        if (GestureManagerVR.isGesturing)
            return;
        GestureManagerVR.setInputFocus(this);
    }
}
