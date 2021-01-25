using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleAnimator : MonoBehaviour
{
    public Transform chassis;
    public Vehicle vehicle;
    
    private Vector3 velocity = new Vector3(0,0,0);
    void Start() {
    }

    private void updatePosition(Transform current) {
        Vector3 oldPosition = chassis.position;
        chassis.position = Vector3.SmoothDamp(chassis.position, current.position, ref velocity, 3f);
        velocity = (current.position - oldPosition) / Time.deltaTime;
    }

    private void updateRotation(Transform current) {
        chassis.rotation = Quaternion.Slerp(chassis.rotation, current.rotation, 0.05f);
    }

    private void updateTransform(Transform current) {
        updatePosition(current);
        updateRotation(current);
    }

    void Update() {
        Transform currentTransform = vehicle.getTransform();
        updateTransform(currentTransform);
    }
}
