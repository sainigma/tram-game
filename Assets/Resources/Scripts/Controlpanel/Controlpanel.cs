using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Controlpanel : MonoBehaviour, StateCollector
{
    public GameObject buttonPrefab;
    public GameObject dialPrefab;
    public GameObject rotaryPrefab;

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

    public List<Button> getButtons() {
        return buttons;
    }

    public bool isInitialized() {
        return initialized;
    }

    private GameObject spawnPrefab(GameObject prefab, (float, float, float) pos, (float, float, float) rot, string name) {
        GameObject newObject = Instantiate(prefab, new Vector3(), Quaternion.identity);
        newObject.transform.parent = this.transform;
        newObject.transform.localPosition = new Vector3(pos.Item1, pos.Item2, pos.Item3);
        newObject.transform.localRotation = Quaternion.Euler(rot.Item1, rot.Item2, rot.Item3);
        newObject.name = name;
        return newObject;
    }

    private void spawnButton(ButtonParameters btn) {
        GameObject newButton = spawnPrefab(buttonPrefab, (btn.x, btn.y, 0), (90, 0, 0), "button: " + btn.label);
        Button button = newButton.GetComponent<Button>();

        button.setLabel(btn.label);
        button.setColor(colors[btn.color]);
        button.init();

        buttons.Add(button);
        if (btn.label != " ") {
            namedButtons[btn.label] = button;
        }
    }

    private void spawnDial(DialParameters dialConfig) {
        GameObject newDial = spawnPrefab(dialPrefab, (dialConfig.x, dialConfig.y, 0), (0, 0, 0), "dial: " + dialConfig.label);
        Dial newDialLogic = newDial.GetComponent<Dial>();
        newDialLogic.init(dialConfig);
        namedDials.Add(dialConfig.label, newDialLogic);
    }

    private void spawnRotarySelector(RotarySelectorParameters selector, List<string> labels) {
        GameObject newSelector = spawnPrefab(rotaryPrefab, (selector.x, selector.y, 0), (90, 0, 0), "selector: " + selector.name);
        RotarySelector selectorLogic = newSelector.GetComponent<RotarySelector>();
        selectorLogic.init(selector, labels);
    }

    public void init() {
        if (initialized) {
            return;
        }
        foreach (HSVColor hsv in config.colors) {
            colors.Add(Color.HSVToRGB(hsv.h, hsv.s, hsv.v));
        }
        foreach (ButtonParameters btn in config.buttons) {
            spawnButton(btn);
        }
        foreach (DialParameters dial in config.dials) {
            spawnDial(dial);
        }
        foreach (RotarySelectorParameters selector in config.rotaryselectors) {
            spawnRotarySelector(selector, config.rotarylabels[selector.labels].labels);
        }
        initialized = true;
    }

    void Start() {
        config = ControlpanelParameters.CreateFromJSON(jsonConfig.ToString());
        init();
        //namedButtons["lähtee"].setEmission(true);
    }
}
