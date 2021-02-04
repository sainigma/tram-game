using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpedesGames : MonoBehaviour {
    public float volume;

    private Controlpanel controlpanel;
    private List<Button> buttons;
    private int buttonsSize;

    private Stack<int> buttonOrder = new Stack<int>();

    private Queue<int> buttonChecklist = new Queue<int>();
    private Queue<int> tone = new Queue<int>();

    private List<double> numbers = new List<double>();

    private AudioSource audioSource;

    private float twelthRootOfTwo = Mathf.Pow(2f, 1.0f / 12f);

    private void generateNumbers(int size) {
        octave += 1;
        int previous = previousButton;
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
        int tone = value % 11;
        int multiplier = Mathf.RoundToInt((float)value / 12f);
        if (tone == 1 || tone == 3 || tone == 6 || tone == 8 || tone == 10) {
            tone++;
        }
        audioSource.pitch = Mathf.Pow(twelthRootOfTwo, tone - 48 + (multiplier + octave) * 12);
        audioSource.Play();

        buttons[value].setEmission(true);
        buttonChecklist.Enqueue(buttons[value].id);
        if (buttonOrder.Count == 0) {
            generateNumbers(12);
        }
    }

    void Start() {
        controlpanel = GetComponent<Controlpanel>();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = Resources.Load<AudioClip>("Sounds/tone");
        audioSource.loop = true;
        audioSource.volume = volume;
        audioSource.spatialize = true;
        audioSource.minDistance = 0.1f;
        audioSource.maxDistance = 1.0f;
    }

    private bool initialized = false;
    private void init() {
        if (initialized) {
            return;
        }
        if (!controlpanel.isInitialized()) {
            return;
        }
        initialized = true;
        buttons = controlpanel.getButtons();
        buttonsSize = buttons.Count;
    }

    private int points;
    private int octave;
    private float timer;
    private float timerThreshold;
    private int overhead;
    private bool gameActive = false;

    public void startGame() {
        octave = 1;
        buttonOrder = new Stack<int>();
        buttonChecklist = new Queue<int>();
        Random.InitState((int)System.DateTime.Now.Ticks);
        generateNumbers(12);

        setTimer(0);
        setPoints(0);
        timerThreshold = 1f;
        overhead = -1;
        gameActive = true;
    }

    private void setTimer(float value) {
        timer = value;
        controlpanel.namedDials["dialbrakes"].setValueRelative(timer / timerThreshold);
    }

    private void setPoints(int value) {
        points = value;
        timerThreshold -= 0.01f;
        controlpanel.namedDials["dialvelocity"].setValue(points);
    }

    private void endGameLoop() {
        buttons[previousButton].setEmission(false);
        gameActive = false;
        audioSource.Stop();
        setTimer(0);
    }

    private void gameLoop() {
        if (!gameActive) {
            return;
        }

        if (controlpanel.hasNext()) {
            ButtonState userInput = controlpanel.getNext();
            int target = buttonChecklist.Dequeue();
            if (userInput.id == target) {
                setPoints(points + 1);
                if (overhead == 0) {
                    next();
                    setTimer(0);
                } else {
                    overhead--;
                } 
            } else {
                endGameLoop();
            }
        } else if (timer > timerThreshold) {
            next();
            setTimer(0);
            overhead++;
        }
        setTimer(timer += Time.deltaTime);
    }

    void Update() {
        init();
        gameLoop();
        if (!gameActive) {
            if (controlpanel.hasNext()) {
                ButtonState userInput = controlpanel.getNext();
                if (userInput.id == buttons[0].id) {
                    startGame();
                }
            }
        }
    }
}
