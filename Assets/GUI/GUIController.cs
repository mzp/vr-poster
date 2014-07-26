using UnityEngine;
using System;
using System.Collections;

public class GUIController : MonoBehaviour {
    private bool isMenu = false;
    public GUISkin customSkin = null;

    public bool isOculusRift = false;
    private float angleVelocity = 0.0f;
    public float AngleVelocity {
        set {
            this.angleVelocity = value;
            PlayerPrefs.SetFloat("angleVelocity", value);
            PlayerPrefs.Save();
        }
        get {
            return this.angleVelocity != 0.0f ?
                this.angleVelocity :
                PlayerPrefs.GetFloat("angleVelocity", 1.0f);
        }
    }

    public string keywords = null;
    public string Keywords {
        set {
            this.keywords = value;
            PlayerPrefs.SetString("keywords", value);
            PlayerPrefs.Save();
        }
        get {
            return String.IsNullOrEmpty(keywords) ?
                PlayerPrefs.GetString("keywords", "ゆるゆり users") :
                keywords;
        }
    }

    void Update() {
        if (Input.GetButtonDown("Fire3")) {
            isMenu = !isMenu;
        }
    }

    void OnGUI() {
        if(!isMenu) { return; }
        if(customSkin) { GUI.skin = customSkin; }

        GUI.Box(new Rect(0,0,270,Screen.height),"Configure Menu");

        isOculusRift = GUI.Toggle(new Rect(10, 40, 250, 24), isOculusRift, "Enable OculusRift");


        GUI.Label(new Rect(10,80,250,24), String.Format("Angle Velocity: {0}",this.AngleVelocity));
        this.AngleVelocity = GUI.HorizontalSlider(new Rect(10,100,250,24), this.AngleVelocity, 0.0f, 5.0f);

        GUI.Label(new Rect(10,130,250,24), "Keywords");
        this.Keywords = GUI.TextField(new Rect(10,150,250,24), this.Keywords);

        if(GUI.Button(new Rect(10,200,250,24),"Reset")) {
            this.AngleVelocity = 1.0f;
            this.Keywords = "ゆるゆり users";
        }

        if(GUI.Button(new Rect(10,240,250,24),"Hide menu")){
            isMenu = false;
        }
    }

}
