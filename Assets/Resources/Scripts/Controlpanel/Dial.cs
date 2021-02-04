using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dial : MonoBehaviour {

    private GameObject hand;
    private float range;
    private float offset;
    private float maxValue;
    private float currentValue;

    public void setFace(string name) {
        GameObject face = transform.Find("face").gameObject;
        SpriteRenderer sRenderer = face.GetComponent<SpriteRenderer>();
        sRenderer.sprite = Resources.Load<Sprite>("Models/Controlpanel/Dial/Sprites/" + name);
    }

    public void setLogo(bool state) {
        GameObject logo = transform.Find("logo").gameObject;
        logo.SetActive(state);
    }

    public void setValue(float value) {
        float currentValue = value;
        float t = value / maxValue;
        setValueRelative(t);
    }

    public void setValueRelative(float t) {
        float rot = offset + t * range;
        hand.transform.localRotation = Quaternion.Euler(0,0,rot);
    }

    public void init(DialParameters dial) {
        hand = transform.Find("dial.hand").gameObject;
        setFace(dial.label);
        setLogo(dial.logo);
        range = dial.range;
        offset = dial.offset;
        maxValue = dial.maxValue;
        setValue(0);
    }
}
