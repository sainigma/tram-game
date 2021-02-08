using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotarySelector : MonoBehaviour, InteractionInterface {

    private GameObject handle;
    private GameObject dial;

    public void init(RotarySelectorParameters config, List<string> labelsText) {
        handle = this.transform.Find("rotaryselector.handle").gameObject;
        dial = this.transform.Find("rotaryselector.dial").gameObject;
        
        setLabels(labelsText);
        setNameTag(config.name);

        GameObject[] arr = {handle, dial};
        arr[config.handle].SetActive(true);
    }

    public void interact() {

    }
    public void noninteract() {

    }

    private void setLabel(GameObject labelObject, string text) {
        labelObject.GetComponent<TextMesh>().text = text;
    }

    private void setLabels(List<string> labelsText) {
        Transform labelsOrigin = this.transform.Find("labels");
        for (int i = 0; i < labelsOrigin.childCount; i++) {
            setLabel(labelsOrigin.GetChild(i).gameObject, labelsText[i]);
        }
    }

    private void setNameTag(string name) {
        GameObject namePlate = this.transform.Find("nameplate").gameObject;
        TextMesh nameMesh = this.transform.Find("name").GetComponent<TextMesh>();
        if (name.Length == 0) {
            namePlate.SetActive(false);
            nameMesh.gameObject.SetActive(false);
            return;
        }
        nameMesh.text = name;
    }
}
