using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public Vector3 velocity;
    public Vector3 acceleration;
    public float maxSpeed;
    private Vector2 targetVelocity;
    private float restitutionCoeff = 0.05f;
    private bool isGrounded = false;

    private Transform viewport;

    private Transform previousTransform = null;
    private Vector3 previousGroundPosition = new Vector3(0,0,0);

    void Start() {
        velocity = new Vector3(0, 0, 0);
        acceleration = new Vector3(0, -9.81f, 0);
        targetVelocity = new Vector2(0, 0);
        maxSpeed = 5f;
        viewport = transform.Find("Offset").Find("Viewport");
    }
    public void move(Vector2 movement) {
        Vector2 direction = movement.normalized;
        float speed = Mathf.Clamp(movement.magnitude, -1f, 1f) * maxSpeed;
        targetVelocity = direction * speed;
    }

    public void turn(float angle) {
        transform.Rotate(new Vector3(0, angle * 0.1f, 0), Space.Self);
    }

    public void look(float angle) {
        angle = -angle;
        float oldRotation = viewport.rotation.eulerAngles.x;
        float newRotation = oldRotation + angle;
        if (newRotation > 270f || newRotation < 90f) {
            viewport.Rotate(new Vector3(angle * 0.1f, 0, 0), Space.Self);
        }
    }

    public void jump() {
        if (isGrounded) {
            velocity.y = 3f;
            isGrounded = false;
        }
    }

    private void accelerate() {
        velocity.x = targetVelocity.x;
        velocity.z = targetVelocity.y;
        velocity = transform.rotation * velocity;
    }

    void Update() {
        testGrounded();
        accelerate();
        velocity += acceleration * Time.deltaTime;
        moveToPosition(velocity * Time.deltaTime, true);
    }

    private void moveToPosition(Vector3 positionDelta, bool first) {
        if (!hasCollisions(transform.position + positionDelta)) {
            this.transform.position += positionDelta;
        } else if (first){
            moveToPosition(velocity * Time.deltaTime, false);
        } else {
            //push away from hit
        }
    }

    private bool hasCollisions(Vector3 position) {
        RaycastHit [] hits;
        float height = 1.76f;
        float radius = 0.5f;
        float offset = 0.5f;

        Vector3 direction = (position - transform.position).normalized;

        Vector3 p1 = position + Vector3.up * height * 0.5f;
        Vector3 p2 = p1 + Vector3.up * (height - offset);
        float positionDelta = (transform.position - position).magnitude;
        if (positionDelta < 0.05f) {
            positionDelta = 0.05f;
        }

        hits = Physics.CapsuleCastAll(p1, p2, radius, direction, positionDelta);
        if (hits.Length > 0) {
            Vector3 newVelocity = (direction + hits[0].normal) * velocity.magnitude;
            newVelocity.y = 0f;
            velocity = newVelocity;
            return true;
        }
        return false;
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

    private void testGrounded() {
        //Muuta käyttämään capsule castia
        if (Physics.Raycast(this.transform.position, this.transform.root.up, out RaycastHit hitUp, 0.5f)) {
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
            acceleration.y = 0f;
            if (velocity.y < -0.1f) {
                velocity.y = -velocity.y * restitutionCoeff;
            } else if (velocity.y < 0.1f){
                velocity.y = 0f;
            }
        } else {
            acceleration.y = -9.81f;
        }
    }
}