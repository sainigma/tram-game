using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Controlpanel : MonoBehaviour, StateCollector
{
    public GameObject buttonPrefab;
    public GameObject dialPrefab;

    public TextAsset jsonConfig;

    private bool initialized = false;
    private ControlpanelParameters config;
    private List<Button> buttons = new List<Button>();
    private List<Color> colors = new List<Color>();
    private Dictionary<string, Button> namedButtons = new Dictionary<string, Button>();
    public Dictionary<string, Dial> namedDials = new Dictionary<string, Dial>();

    private Queue<ButtonState> buttonQueue = new Queue<ButtonState>();
    public void report(int id, bool state) {
        buttonQueue.Enqueue(new ButtonState(id, state));
    }
    public bool hasNext() {
        return buttonQueue.Count > 0;
    }
    public ButtonState getNext() {
        return buttonQueue.Dequeue();
    }

    private void spawnButton(float x, float y, int color, string label) {
        GameObject newButton = Instantiate(buttonPrefab, new Vector3(), Quaternion.identity);
        newButton.transform.parent = this.transform;
        newButton.transform.localPosition = new Vector3(x, y, 0);
        newButton.transform.localRotation = Quaternion.Euler(90, 0, 0);
        newButton.name = "button: " + label;
        Button button = newButton.GetComponent<Button>();

        button.setLabel(label);
        button.setColor(colors[color]);
        button.init();

        buttons.Add(button);
        if (label != " ") {
            namedButtons[label] = button;
        }
    }

    public List<Button> getButtons() {
        return buttons;
    }

    public bool isInitialized() {
        return initialized;
    }

    private void spawnDial(DialParameters dialConfig) {
        GameObject newDial = Instantiate(dialPrefab, new Vector3(), Quaternion.identity);
        newDial.transform.parent = this.transform;
        newDial.transform.localPosition = new Vector3(dialConfig.x, dialConfig.y, 0);
        newDial.transform.localRotation = Quaternion.Euler(0, 0, 0);

        Dial newDialLogic = newDial.GetComponent<Dial>();
        newDialLogic.init(dialConfig);
        namedDials.Add(dialConfig.label, newDialLogic);
    }


    public void init() {
        if (initialized) {
            return;
        }
        foreach (HSVColor hsv in config.colors) {
            colors.Add(Color.HSVToRGB(hsv.h, hsv.s, hsv.v));
        }
        foreach (ButtonParameters btn in config.buttons) {
            spawnButton(btn.x, btn.y, btn.color, btn.label);
        }
        foreach (DialParameters dial in config.dials) {
            spawnDial(dial);
        }
        initialized = true;
    }

    void Start() {
        config = ControlpanelParameters.CreateFromJSON(jsonConfig.ToString());
        init();
        //namedButtons["lähtee"].setEmission(true);
    }
}
