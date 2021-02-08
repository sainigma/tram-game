using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HSVColor {
  public float h;
  public float s;
  public float v;
}

[System.Serializable]
public class ButtonParameters {
  public float x, y;
  public int color;
  public string label;
}

[System.Serializable]
public class DialParameters {
  public float x, y;
  public string label;
  public float range, offset, maxValue;
  public bool logo;
}

[System.Serializable]
public class RotaryLabels {
  public List<string> labels;
}

[System.Serializable]
public class RotarySelectorParameters {
  public float x, y;
  public string name;
  public int handle, labels;
}

[System.Serializable]
public class ControlpanelParameters {

  public int status;
  public List<HSVColor> colors;
  public List<ButtonParameters> buttons;
  public List<DialParameters> dials;
  public List<RotaryLabels> rotarylabels;
  public List<RotarySelectorParameters> rotaryselectors;

  public static ControlpanelParameters CreateFromJSON(string jsonString) {
    return JsonUtility.FromJson<ControlpanelParameters>(jsonString);
  }
}