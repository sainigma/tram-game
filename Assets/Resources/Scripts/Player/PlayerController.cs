using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameControls playerControls;
    private PlayerMovement playerMovement;
    public bool tankControls;

    private void Start() {
        playerMovement = transform.GetComponent<PlayerMovement>();
    }

    private void Awake() {
        playerControls = new GameControls();
    }

    private void OnEnable() {
        playerControls.Enable();
    }

    private void OnDisable() {
        playerControls.Disable();
    }

    void Update() {
        playerMovement.move(playerControls.fpsPlayer.movement.ReadValue<Vector2>());
        playerMovement.turn(playerControls.fpsPlayer.turn.ReadValue<float>());
        playerMovement.look(playerControls.fpsPlayer.look.ReadValue<float>());
    }
}
