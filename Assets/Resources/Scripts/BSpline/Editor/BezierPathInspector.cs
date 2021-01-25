using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BezierPath))]
public class BezierPathInspector : Editor {

    private BezierPath path;
    private Transform handleTransform;
    private Quaternion handleRotation;

    private const float handleSize = 0.04f;
    private const float pickSize = 0.06f;
    private int selectedIndex = -1;

    private Vector3 previousEuler = Vector3.zero;

    private Vector3 ShowPoint(int index) {
        Vector3 point = handleTransform.TransformPoint(path.GetControlPoint(index));
        float worldSize = HandleUtility.GetHandleSize(point);

        Handles.color = Color.white;
        if (Handles.Button(point, handleRotation, worldSize * handleSize, worldSize * pickSize, Handles.DotHandleCap)) {
          selectedIndex = index;
          Repaint();
        }
        if (selectedIndex == index) {
          EditorGUI.BeginChangeCheck();
          point = Handles.DoPositionHandle(point, handleRotation);
          if (EditorGUI.EndChangeCheck()) {
              Undo.RecordObject(path, "Move Point");
              EditorUtility.SetDirty(path);
              path.SetControlPoint(index, handleTransform.InverseTransformPoint(point));
          }
        }

        return point;
    }

    private void drawCurve(Vector3[] points) {
        Handles.color = Color.white;
        Vector3 start = points[0];
        for (int i=1; i < points.Length; i++) {
            Handles.DrawLine(start, points[i]);
            start = points[i];
        }
    }

    private void OnSceneGUI() {
        path = target as BezierPath;
        handleTransform = path.transform;
        handleRotation = Tools.pivotRotation == PivotRotation.Local ? handleTransform.rotation : Quaternion.identity;

        Vector3 p0 = ShowPoint(0);
        for (int i = 1; i < path.GetControlPointCount(); i += 3) {
          Vector3 p1 = ShowPoint(i);
          Vector3 p2 = ShowPoint(i + 1);
          Vector3 p3 = ShowPoint(i + 2);

          Handles.color = new Color(1f ,165 / 255f, 0);
          Handles.DrawLine(p0, p1);
          Handles.DrawLine(p2, p3);

          if (path.debug) {
            Handles.DrawBezier(p0, p3, p1, p2, Color.green, null, 2f);
          }
          p0 = p3;
        }
        Vector3[] approximation = path.GetApproximation();
        if ((path.transform.position - approximation[0]).magnitude > 0.1f || (path.transform.rotation.eulerAngles - previousEuler).magnitude > 0.1f) {
          path.SetApproximation();
          previousEuler = path.transform.rotation.eulerAngles;
        }
        drawCurve(path.GetApproximation());
    }

  public override void OnInspectorGUI()
  {
    path = target as BezierPath;
    if (selectedIndex >= 0 && selectedIndex < path.GetControlPointCount()) {
      DrawSelectedPointInspector();
    }
    
    if (GUILayout.Button("Update")) {
      path.SetApproximation();
    }

    if (GUILayout.Button("Add Curve")) {
      Undo.RecordObject(path, "Add Curve");
      path.AddCurve();
      EditorUtility.SetDirty(path);
    }
    if (GUILayout.Button("Remove Curve")) {
      Undo.RecordObject(path, "Remove Curve");
      path.RemoveCurve();
      EditorUtility.SetDirty(path);
    }
    int previousResolution = path.resolution;
    path.resolution = (int)EditorGUILayout.Slider(path.resolution, 3, 20);
    if (path.resolution != previousResolution) {
      path.SetApproximation();
    }
    path.debug = EditorGUILayout.ToggleLeft("Debug", path.debug);

    if (path.debug) {
      DrawDefaultInspector();
    }
    GUILayout.Label("Step length: " + path.stepLength);
  }

  private void DrawSelectedPointInspector() {
    GUILayout.Label("Selected point");
    EditorGUI.BeginChangeCheck();
    Vector3 point = EditorGUILayout.Vector3Field("Position", path.GetControlPoint(selectedIndex));
    if (EditorGUI.EndChangeCheck()) {
      Undo.RecordObject(path, "Move point");
      EditorUtility.SetDirty(path);
      path.SetControlPoint(selectedIndex, point);
    }
  }
}
