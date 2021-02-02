using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Controlpanel : MonoBehaviour
{
    public GameObject buttonPrefab;
    public GameObject dialPrefab;

    public TextAsset jsonConfig;

    private bool initialized = false;
    private ControlpanelParameters config;
    private List<Button> buttons = new List<Button>();
    private List<Color> colors = new List<Color>();
    private Dictionary<string, Button> namedButtons = new Dictionary<string, Button>();

    private void spawnButton(float x, float y, int color, string label) {
        GameObject newButton = Instantiate(buttonPrefab, new Vector3(), Quaternion.identity);
        newButton.transform.parent = this.transform;
        newButton.transform.localPosition = new Vector3(x, y, 0);
        newButton.transform.localRotation = Quaternion.Euler(90, 0, 0);

        Button button = newButton.GetComponent<Button>();

        button.setLabel(label);
        button.setColor(colors[color]);
        button.init();

        buttons.Add(button);
        if (label != " ") {
            namedButtons[label] = button;
        }
    }

    private void spawnDial(DialParameters dialConfig) {
        GameObject newDial = Instantiate(dialPrefab, new Vector3(), Quaternion.identity);
        newDial.transform.parent = this.transform;
        newDial.transform.localPosition = new Vector3(dialConfig.x, dialConfig.y, 0);
        newDial.transform.localRotation = Quaternion.Euler(0, 0, 0);

        newDial.GetComponent<Dial>().init(dialConfig);
    }


    public void init() {
        if (initialized) {
            return;
        }
        initialized = true;
        foreach (HSVColor hsv in config.colors) {
            colors.Add(Color.HSVToRGB(hsv.h, hsv.s, hsv.v));
        }
        foreach (ButtonParameters btn in config.buttons) {
            spawnButton(btn.x, btn.y, btn.color, btn.label);
        }
        foreach (DialParameters dial in config.dials) {
            spawnDial(dial);
        }
    }

    void Start() {
        config = ControlpanelParameters.CreateFromJSON(jsonConfig.ToString());
        init();
        //namedButtons["lähtee"].setEmission(true);
    }

    private float timer = 0;
    private int previous = -1;
    void Update() {
        if (initialized) {
            if (timer > 0.5f) {
                int random = (int)Random.Range(0, buttons.Count);
                buttons[random].toggleEmission();
                if (previous != -1) {
                    buttons[previous].toggleEmission();
                }
                previous = random;
                timer = 0;
            }
            timer += Time.deltaTime;
        }
    }
}
