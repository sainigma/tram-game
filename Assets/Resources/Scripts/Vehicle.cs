using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour {

    public float axleWidth;
    public float wheelbase;
    public float wheelRadius;

    private Rigidbody rb; 
    private WheelCollider[] wheels = new WheelCollider[4];

    private WheelCollider spawnWheel(bool right, bool front) {
        string a = front ? "front" : "rear";
        string b = right ? "right" : "left";

        GameObject wheel = new GameObject(transform.name + " " + a + " wheel " + b);
        wheel.transform.parent = this.transform;
        wheel.transform.localPosition = new Vector3((right ? 1f : -1f) * axleWidth / 2f, wheelRadius, (front ? 1f : -1f) * wheelbase / 2f);

        WheelCollider collider = wheel.AddComponent<WheelCollider>();
        collider.radius = wheelRadius;
        collider.suspensionDistance = 0f;
        collider.mass = 1f;
        return collider;
    }

    private void createWheels() {
        for (int i = 0; i < 4; i++) {
            wheels[i] = spawnWheel(i % 2 == 0, i < 2);
        }
    }

    void Start() {
        rb = GetComponent<Rigidbody>();
        createWheels();
        wheels[0].motorTorque = 3f;
        wheels[1].motorTorque = 3f;
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

    void Update() {
        
    }
}
