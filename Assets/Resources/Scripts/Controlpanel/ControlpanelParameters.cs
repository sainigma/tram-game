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
  public float range, offset;
}

[System.Serializable]
public class ControlpanelParameters {


  public int status;
  public List<HSVColor> colors;
  public List<ButtonParameters> buttons;

  public static ControlpanelParameters CreateFromJSON(string jsonString) {
    return JsonUtility.FromJson<ControlpanelParameters>(jsonString);
  }
}