using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour {

    public float axleWidth;
    public float wheelbase;
    public float wheelRadius;

    public float mass;

    private Rigidbody rb; 
    private WheelCollider[] wheels = new WheelCollider[4];

    private WheelCollider spawnWheel(bool right, bool front) {
        string a = front ? "front" : "rear";
        string b = right ? "right" : "left";

        GameObject wheel = new GameObject(transform.name + " " + a + " wheel " + b);
        wheel.transform.parent = this.transform;
        wheel.transform.localPosition = new Vector3((right ? 1f : -1f) * axleWidth / 2f, wheelRadius, (front ? 1f : -1f) * wheelbase / 2f);
        wheel.layer = gameObject.layer;

        WheelCollider collider = wheel.AddComponent<WheelCollider>();
        var suspension = collider.suspensionSpring;
        suspension.spring = 5 * 0.25f * mass * 9.81f;
        suspension.damper = 0.2f;
        suspension.targetPosition = 1f;
        collider.suspensionSpring = suspension;
        collider.radius = wheelRadius;
        collider.suspensionDistance = 0.001f;
        collider.mass = 0.01f * mass;

        collider.ConfigureVehicleSubsteps(5, 12, 15);

        return collider;
    }

    private void createWheels() {
        for (int i = 0; i < 4; i++) {
            wheels[i] = spawnWheel(i % 2 == 0, i < 2);
        }
    }

    void Start() {
        rb = GetComponent<Rigidbody>();
        rb.mass = 0.96f * mass;
        createWheels();
    }

    public void setTorque(float torque) {
        if (Mathf.Abs(torque - wheels[0].motorTorque) > 0.5f) {
            wheels[0].motorTorque = torque;
            wheels[1].motorTorque = torque;
        }
    }

    public void setTurnRadius(float angle) {
        if (Mathf.Abs(angle) < 1f) {
            angle = 0f;
        }
        wheels[0].steerAngle = angle;
        wheels[1].steerAngle = angle;
        wheels[2].steerAngle = -angle;
        wheels[3].steerAngle = -angle;
    }

    public void bump() {
        //rb.AddForceAtPosition(transform.up * 1f, new Vector3(0,0,wheelbase), ForceMode.Impulse);
    }

    public Transform getTransform() {
        return rb.transform;
    }

    void Update() {
        
    }
}
