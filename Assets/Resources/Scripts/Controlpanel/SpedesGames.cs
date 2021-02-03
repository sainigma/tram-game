using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpedesGames : MonoBehaviour {
    private Controlpanel controlpanel;
    private List<Button> buttons;
    private int buttonsSize;

    private Stack<int> buttonOrder = new Stack<int>();

    private List<double> numbers = new List<double>();

    private AudioSource audioSource;

    private void generateNumbers(int size) {
        int previous = -1;
        for (int i = 0; i < size; i++) {
            float u1 = 1.0f - Random.value;
            float u2 = 1.0f - Random.value;

            float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);
            float randNormal = randStdNormal / 5f + 0.5f;
            if (randNormal < 0) {
                randNormal = 0;
            } else if (randNormal > 1f) {
                randNormal = 1f;
            }
            int value = (int)(randNormal * (float)buttonsSize - 1);
            if (value != previous && value < buttonsSize && value >= 0) {
                buttonOrder.Push(value);
                previous = value;
            } else {
                i--;
            }
        }

    }    

    private int previousButton = 0;
    private void next() {
        buttons[previousButton].setEmission(false);
        int value = buttonOrder.Pop();
        previousButton = value;

        audioSource.pitch = 0.5f + 0.5f * value / buttonsSize;
        audioSource.Play();

        buttons[value].setEmission(true);
        if (buttonOrder.Count == 0) {
            generateNumbers(10);
        }
    }

    void Start() {
        controlpanel = GetComponent<Controlpanel>();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = Resources.Load<AudioClip>("Sounds/tone");
        audioSource.loop = true;
        audioSource.volume = 0.2f;
        audioSource.spatialize = true;
    }

    private void init() {
        if (!controlpanel.isInitialized()) {
            return;
        }
        initialized = true;
        
        
        buttons = controlpanel.getButtons();
        buttonsSize = buttons.Count;
        Debug.Log(buttonsSize);

        Random.InitState((int)System.DateTime.Now.Ticks);
        generateNumbers(10);
    }

    private float timer = 0;
    private bool initialized = false;
    void Update() {
        if (!initialized) {
            init();
        } else {
            if (timer > 0.5f) {
                int random = (int)Random.Range(0, buttons.Count);
                next();
                timer = 0;
            }
            timer += Time.deltaTime;
        }
        
    }
}
