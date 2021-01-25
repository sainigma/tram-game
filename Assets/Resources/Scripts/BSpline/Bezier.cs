using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bezier : MonoBehaviour
{
    public static Vector3 GetPointOld(Vector3 p0, Vector3 p1, Vector3 p2, float t) {
        return Vector3.Lerp(
            Vector3.Lerp(p0, p1, t),
            Vector3.Lerp(p1, p2, t),
            t
        );
    }

    public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) {
        t = Mathf.Clamp01(t);
        return (1 + t * (-3 + (3 - t) * t)) * p0 +
            t * (3 + t * (-6 + 3 * t)) * p1 +
            3 * (1 - t) * t * t * p2 +
            t * t * t * p3;
    }

    public static Vector3 GetVelocity(Vector3 p0, Vector3 p1, Vector3 p2, float t) {
        return 2f * ((1f - t) * (p1 - p0) + t * (p2 - p1)); 
    }
}
