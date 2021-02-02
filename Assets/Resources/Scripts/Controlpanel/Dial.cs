using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dial : MonoBehaviour {

    private GameObject hand;

    public void setFace(string name) {
        GameObject face = transform.Find("face").gameObject;
        SpriteRenderer sRenderer = face.GetComponent<SpriteRenderer>();
        sRenderer.sprite = Resources.Load<Sprite>("Models/Controlpanel/Dial/Sprites/" + name);
    }

    public void setLogo(bool state) {
        GameObject logo = transform.Find("logo").gameObject;
        logo.SetActive(state);
    }

    public void init(DialParameters dial) {
        hand = transform.Find("dial.hand").gameObject;
        setFace(dial.label);
        setLogo(dial.logo);
    }
}
