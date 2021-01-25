using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathRigidbodyFollower : MonoBehaviour {
    public float force;
    public BezierPath path;
    private Rigidbody rb;
    private Vector3[] controlPoints;
    private bool derailed = false;
    public bool softFollower;
    public int index;
    public float speed;
    private Vector3 targetDirection;
    private Vehicle vehicle;

    void Start() {
        rb = GetComponent<Rigidbody>();
        vehicle = GetComponent<Vehicle>();
        controlPoints = path.GetApproximation();
        index = -1;
    }

    private void nextIndex() {
        if (index + 1 < controlPoints.Length) {
            index++;
            targetDirection = (controlPoints[index] - rb.position).normalized;
        } else {
            //vaihda ohjauskäyrää
        }
    }

    private void checkForControlpointChange() {
        if (index < 0 || (rb.position - controlPoints[index]).magnitude < 1f) {
            nextIndex();
        }
    }

    private void moveTowards(Vector3 target) {
        Vector3 direction = (target - rb.position).normalized;
        if (direction != Vector3.zero) {
            //Quaternion newRotation = Quaternion.LookRotation(direction, rb.transform.up);
            Debug.DrawLine(rb.position + new Vector3(0,0.5f,0), rb.position + 10 * rb.transform.forward + new Vector3(0,0.5f,0), Color.red);
            Debug.DrawLine(rb.position + new Vector3(0,0.5f,0), rb.position + 10 * direction + new Vector3(0,0.5f,0), Color.blue);
            float angle = Vector3.Angle(rb.transform.forward, direction);
            vehicle.setTurnRadius((Vector3.Cross(rb.transform.forward, direction).y > 0) ? angle : -angle);
        }
    }

    private void checkForDerailing() {
        Vector3 currentDirection = (controlPoints[index] - rb.position).normalized;
        float angle = Vector3.Angle(targetDirection, currentDirection);
        if (angle > 10f) {
            if (softFollower) {
                nextIndex();
            } else {
                derailed = true;
            }
        }

    }

    void FixedUpdate() {
        if (!derailed) {
            checkForControlpointChange();
            moveTowards(controlPoints[index]);
            Debug.DrawLine(controlPoints[index], controlPoints[index] + new Vector3(0,10,0), Color.black);
            Debug.DrawLine(rb.position, controlPoints[index], Color.white);
            speed = rb.velocity.magnitude * 3.6f;
            checkForDerailing();
        }
    }
}
