using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class BezierPath : MonoBehaviour
{
    [SerializeField]
    public Vector3[] controlPoints;
    public Vector3[] approximation;

    public BezierPath previous = null;

    public int resolution = 12;
    public float stepLength = -1;
    public bool debug = false;

    public int GetControlPointCount() {
        return controlPoints.Length;
    }

    public void SetControlPoint(int index, Vector3 position) {
        controlPoints[index] = position;
        ConstrainPoints(index);
        SetApproximation();
    }

    public void SetApproximation() {
        ApproximatedCurve curveApproximation = new ApproximatedCurve(this);
        approximation = curveApproximation.getPoints();
        stepLength = curveApproximation.getStepLength();
    }

    public Vector3[] GetApproximation() {
        return approximation;
    }

    private void ConstrainPoints(int index) {
        int modeIndex = (index + 1) / 3;
        if (modeIndex == 0 || modeIndex == GetCurveCount()) {
            return;
        }
        int middleIndex = modeIndex * 3;
        int fixedIndex, enforcedIndex;
        if (index <= middleIndex) {
            fixedIndex = middleIndex - 1;
            enforcedIndex = middleIndex + 1;
        } else {
            fixedIndex = middleIndex + 1;
            enforcedIndex = middleIndex - 1;
        }

        Vector3 middle = controlPoints[middleIndex];
        Vector3 enforcedTangent = middle - controlPoints[fixedIndex];
        controlPoints[enforcedIndex] = middle + enforcedTangent;
    }

    public Vector3 GetControlPoint(int index) {
        return controlPoints[index];
    }

    public Vector3 GetPoint(float t, int index) {
        int offset = 3 * index;
        return transform.TransformPoint(Bezier.GetPoint(controlPoints[offset], controlPoints[offset + 1], controlPoints[offset + 2], controlPoints[offset + 3], t));
    }

    public Vector3 GetVelocity(float t, int index) {
        int offset = 3 * index;
        return transform.TransformPoint(Bezier.GetVelocity(controlPoints[offset], controlPoints[offset + 1], controlPoints[offset + 2], t)) - transform.position;
    }

    public Vector3 GetDirection(float t, int index) {
        return GetVelocity(t, index).normalized;
    }

    public void AddCurve() {
        int size = controlPoints.Length;
        Vector3 point = controlPoints[size - 1];
        Vector3 direction = GetControlPoint(GetControlPointCount() - 1) - GetControlPoint(GetControlPointCount() - 2);
        Array.Resize(ref controlPoints, size + 3);

        for (int i = 0; i < 3; i++) {
            point += direction;
            controlPoints[size + i] = point;
        }
        SetApproximation();
    }

    public void RemoveCurve() {
        Array.Resize(ref controlPoints, GetControlPointCount() - 3);
        SetApproximation();
    }

    public int GetCurveCount() {
        return (controlPoints.Length - 1) / 3; 
    }

    public void Reset() {
        controlPoints = new Vector3[] {
            new Vector3(0f, 0f, 0f),
            new Vector3(0f, 0f, 1f),
            new Vector3(0f, 0f, 2f),
            new Vector3(0f, 0f, 3f)
        };
        resolution = 12;
        SetApproximation();
    }

    private void drawApproximation(Vector3[] points) {
        Vector3 offset = new Vector3(0, 0.1f, 0);
        Vector3 start = points[0];
        for (int i=1; i < points.Length; i++) {
            Debug.DrawLine(start + offset, points[i] + offset, Color.black);
            start = points[i];
        }
    }

    void FixedUpdate() {
        drawApproximation(approximation);
    }
}