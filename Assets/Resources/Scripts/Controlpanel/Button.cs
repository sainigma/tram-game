using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonState {
    public int id;
    public bool state;
    public ButtonState(int id, bool state) {
        this.id = id;
        this.state = state;
    }
}

public class Button : MonoBehaviour, InteractionInterface
{
    public Color color;
    public string label;
    public int clicks = 0;

    private bool emission = false;

    private bool toggleSwitch = false;


    public int id;
    private GameObject lens;
    private GameObject buttonLight;
    private TextMesh textMesh;
    private StateCollector stateCollector;

    public void interact() {
        clicks += 1;
        //setEmission(true);
        lens.transform.localPosition = new Vector3(0, -0.002f, 0);
        textMesh.transform.localPosition = new Vector3(0, 0.00472f, 0);
        stateCollector.report(id, true);
    }

    public void noninteract() {
        //setEmission(false);
        lens.transform.localPosition = new Vector3(0, 0, 0);
        textMesh.transform.localPosition = new Vector3(0, 0.00672f, 0);
        stateCollector.report(id, false);
    }

    private void SetMaterials() {
        float hue, S, V;
        Color.RGBToHSV(color, out hue, out S, out V);

        Material foreground = lens.GetComponent<MeshRenderer>().materials[0];
        Material background = lens.GetComponent<MeshRenderer>().materials[1];
        background.color = color;

        V = 0.5f;
        Color b = Color.HSVToRGB(hue, S, V);
        b.a = 0.5f;
        foreground.color = b;
        
        S = 0.2f;
        V = 1f;
        background.SetColor("_EmissionColor", Color.HSVToRGB(hue, S, V));
    }

    private void SpawnLight() {
        buttonLight = new GameObject("buttonlight");
        buttonLight.transform.parent = this.transform;
        buttonLight.transform.localPosition = new Vector3(0, 0.05f, 0);
        Light pointLight = buttonLight.AddComponent<Light>();
        pointLight.intensity = 1.2f;
        pointLight.range = 0.25f;
        pointLight.color = color;
    }

    public void toggleEmission() {
        emission = !emission;
        setEmission(emission);
    }

    public void setEmission(bool active) {
        emission = active;
        Material background = lens.GetComponent<MeshRenderer>().materials[1];
        if (emission) {
            background.EnableKeyword ("_EMISSION");
        } else {
            background.DisableKeyword ("_EMISSION");
        }
        buttonLight.SetActive(emission);
    }

    public void setLabel(string text) {
        label = text.ToUpper();
        GameObject textMeshParent = this.transform.Find("label").gameObject;
        textMesh = textMeshParent.GetComponent<TextMesh>();
        textMesh.text = label;
    }

    public void setColor(Color color) {
        this.color = color;
    }

    public void init() {
        lens = this.transform.Find("button.lens").gameObject;
        SetMaterials();
        SpawnLight();
        setEmission(false);
        setLabel(label);
    }

    void Start() {
        id = this.gameObject.GetInstanceID();
        stateCollector = this.transform.parent.gameObject.GetComponent<StateCollector>();
    }
}
