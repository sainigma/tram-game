using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Button))]
public class ButtonInspector : Editor {
  private Button button;

  public override void OnInspectorGUI() {
    button = target as Button;
    DrawDefaultInspector();
    
    if (GUILayout.Button("Toggle emission")) {
      button.toggleEmission();
    }
  }
}