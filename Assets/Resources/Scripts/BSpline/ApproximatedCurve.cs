using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ApproximatedCurve {
  BezierPath path;
  Vector3[] resultingPoints;
  float length;
  float stepLength;

  public ApproximatedCurve(BezierPath path) {
    this.path = path;
  }

  public Vector3[] getSegment(int index, int steps) {
    Vector3[] points = new Vector3[steps];
    for (int i = 0; i < steps; i++) {
      float t = i / (float)steps;
      points[i] = path.GetPoint(t, index);
      if (i > 0)
      {
        length += (points[i] - points[i - 1]).magnitude;
      }
    }
    return points;
  }

  public Vector3[] getPoints() {
    int steps = path.resolution * 100;
    length = 0;
    List<Vector3> points = new List<Vector3>();
    for (int i = 0; i < path.GetCurveCount(); i++) {
      points.AddRange(getSegment(i, steps));
    }
    Vector3[] result = points.ToArray();
    resultingPoints = simplify(result);
    return resultingPoints;
  }

  public float getStepLength() {
    return stepLength;
  }

  private Vector3[] simplify(Vector3[] points) {
    int segments = path.resolution * path.GetCurveCount();
    stepLength = length / (float)segments;

    Vector3[] result = new Vector3[segments + 1];
    Vector3 lastPoint = points[0];
    result[0] = lastPoint;

    int j = 1;
    for (int i = 1; i < segments; i++) {
      while (j < points.Length && (points[j] - lastPoint).magnitude < stepLength) {
        j++;
      }
      if (j < points.Length) {
        lastPoint = points[j];
        result[i] = lastPoint;
      } else {
        result[i] = lastPoint;
      }
    }
    result[segments] = points[points.Length - 1];
    return result;
  }

  public float getLength() {
    return length;
  }
}