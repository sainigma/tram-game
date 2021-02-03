using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInteraction : MonoBehaviour {
    
    private float cursorDistance = 0.1f;
    private float interactionRange = 10f;
    private GameObject cursor;
    private Camera viewport;

    private bool debounced = true;

    private string[] cursorLabels = {"default", "use", "click", "clicking", "grab", "grabbing"};
    private Dictionary<string, Sprite> cursorSprites = new Dictionary<string, Sprite>();
    
    private InteractionInterface activeInterface;

    void Start() {
        viewport = GetComponent<Camera>();
        cursor = transform.Find("cursor").gameObject;
        loadCursors();
        setCursor("default");
        cursor.SetActive(true);
        Cursor.visible = false;
    }

    private void loadCursors() {
        Sprite[] cursorAtlas = Resources.LoadAll<Sprite>("Sprites/cursors");
        int i = 0;
        foreach (string label in cursorLabels) {
            cursorSprites.Add(label, cursorAtlas[i]);
            i++;
        }
    }

    private void setCursor(string label) {
        if (cursorSprites.ContainsKey(label)) {
            cursor.GetComponent<SpriteRenderer>().sprite = cursorSprites[label];
        }
    }

    private void ScanInteractables() {
        Ray ray = viewport.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo = new RaycastHit();
        bool hit = Physics.Raycast(ray, out hitInfo, interactionRange);
        if (hit) {
            GameObject collision = hitInfo.transform.gameObject;
            if (collision.tag == "Interactable") {
                setCursor("click");
                if (Input.GetMouseButton(0)) {
                    setCursor("clicking");
                    GameObject target = collision.transform.parent.gameObject;
                    debounced = false;
                    activeInterface = target.GetComponent<InteractionInterface>();
                    activeInterface.interact();
                }
            } else {
                setCursor("default");
            }
        }
    }

    private void MoveCursor() {
        Ray ray = viewport.ScreenPointToRay(Input.mousePosition);
        Vector3 unit = ray.GetPoint(1f);
        Vector3 pos = viewport.transform.InverseTransformPoint(unit);
        float t = pos.z / cursorDistance;
        cursor.transform.localPosition = pos / t;
    }

    void Update() {
        MoveCursor();
        if (debounced) {
            ScanInteractables();
        } else if (!Input.GetMouseButton(0)) {
            debounced = true;
            activeInterface.noninteract();
        }
    }
}
