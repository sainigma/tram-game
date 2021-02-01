using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleAnimator : MonoBehaviour
{
    public Transform root;
    public Transform chassis;
    public Transform subChassis;
    public Vehicle vehicle;
    
    private Vector3 velocity = new Vector3(0,0,0);
    void Start() {
    }

    private void updatePosition(Transform current) {
        Vector3 oldPosition = root.position;
        root.position = Vector3.SmoothDamp(root.position, current.position, ref velocity, 3f);
        velocity = (current.position - oldPosition) / Time.deltaTime;
    }

    private void updateRotation(Transform current) {
        /*
        Vector3 currentRotation = current.rotation.eulerAngles;
        
        root.localRotation = Quaternion.Euler(0, currentRotation.y, 0);
        chassis.localRotation = Quaternion.Euler(0, currentRotation.x + 90f, 0);
        subChassis.localRotation = Quaternion.Euler(currentRotation.z, 0, 0);
        
        */
        root.rotation = Quaternion.Slerp(root.rotation, current.rotation, 0.05f);
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
