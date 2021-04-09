using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public Vector3 velocity = new Vector3(0, 0, 0);
    public Vector3 acceleration = new Vector3(0, 0, 0);
    private float restitutionCoeff = 0.05f;
    private bool isGrounded = false;
    private float maxVelocity = 5f;
    private float maxAcceleration = 1f;
    private Transform previousTransform = null;
    private Vector3 previousGroundPosition = new Vector3(0,0,0);

    public bool asd = false;
    void Start() {
        
    }

    public void move(Vector2 direction, float speed) {
        if (!asd) {
            velocity.x = 0f;
            velocity.z = 0f;
            return;
        }
        Vector2 planarVelocity = new Vector2(velocity.x, velocity.z);
        if (planarVelocity.magnitude > maxVelocity) {
            return;
        }
        direction = direction.normalized;
        Vector2 planarAcceleration = direction * maxAcceleration * (speed / 1f);
        acceleration.x = planarAcceleration.x;
        acceleration.z = planarAcceleration.y;
    }

    public void jump() {
        if (isGrounded) {
            velocity.y = 3f;
            isGrounded = false;
        }
    }

    void Update() {
        testGrounded();
        useGravity();
        move(new Vector2(0, 10), 1f);
        velocity += acceleration * Time.deltaTime;
        this.transform.position += velocity * Time.deltaTime;
    }

    private void useGravity() {
        if (isGrounded) {
            acceleration.y = 0f;
        } else {
            acceleration.y = -9.81f;
        }
    }

    private void followGround(Transform ground) {
        if (ground == null) {
            return;
        }
        if (!isGrounded || previousTransform != ground) {
            previousTransform = ground;
            previousGroundPosition = ground.position;
            return;
        }
        Vector3 deltaDistance = ground.position - previousGroundPosition;
        previousTransform = ground;
        this.transform.position += deltaDistance;
        previousGroundPosition = ground.position;
    }

    private void OnCollisionEnter(Collision other) {
        Debug.Log("collision");
    }

    private void testGrounded() {
        if (Physics.Raycast(this.transform.position, this.transform.root.up, out RaycastHit hitUp, 1.5f)) {
            followGround(hitUp.transform);
            this.transform.position = hitUp.point;
            isGrounded = true;
        } else if (Physics.Raycast(this.transform.position, -this.transform.root.up, out RaycastHit hitDown, 0.05f)) {
            followGround(hitDown.transform);
            isGrounded = true;
        } else {
            previousGroundPosition = new Vector3(0, 0, 0);
            isGrounded = false;
        }
        if (isGrounded) {
            if (velocity.y < -0.1f) {
                velocity.y = -velocity.y * restitutionCoeff;
            } else if (velocity.y < 0.1f){
                velocity.y = 0f;
            }
        }
    }

    void FixedUpdate() {
        
    }
}