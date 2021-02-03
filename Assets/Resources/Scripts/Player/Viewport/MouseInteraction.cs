using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInteraction : MonoBehaviour {
    
    private float interactionRange = 10f;
    private Camera viewport;

    private bool debounced = true;

    private InteractionInterface activeInterface;

    void Start() {
        viewport = GetComponent<Camera>();
    }

    private void ScanInteractables() {
        Ray ray = viewport.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo = new RaycastHit();
        bool hit = Physics.Raycast(ray, out hitInfo, interactionRange);
        if (hit) {
            GameObject collision = hitInfo.transform.gameObject;
            if (collision.tag == "Interactable") {
                if (Input.GetMouseButton(0)) {
                    GameObject target = collision.transform.parent.gameObject;
                    Debug.Log(target.name);
                    debounced = false;
                    activeInterface = target.GetComponent<InteractionInterface>();
                    activeInterface.interact();
                }
            }
        }
    }

    void Update() {
        if (debounced) {
            ScanInteractables();
        } else if (!Input.GetMouseButton(0)) {
            debounced = true;
            activeInterface.noninteract();
        }
    }
}
