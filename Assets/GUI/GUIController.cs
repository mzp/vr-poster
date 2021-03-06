﻿using UnityEngine;
using System;
using System.Collections;

public class GUIController : MonoBehaviour {
    public bool isMenu = false;
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

    public string path = null;
    public string Path {
        set {
            this.path = value;
            PlayerPrefs.SetString("path", value);
        }
        get {
            return String.IsNullOrEmpty(path) ?
                PlayerPrefs.GetString("path", Application.dataPath) :
                path;
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

        GUI.Label(new Rect(10,180,250,24), "Path");
        this.Path = GUI.TextField(new Rect(10,200,250,24), this.Path);

        if(GUI.Button(new Rect(10,230,250,24),"Reset")) {
            this.AngleVelocity = 1.0f;
            this.Path = Application.dataPath;
            this.Keywords = "ゆるゆり users";
        }

        if(GUI.Button(new Rect(10,260,250,24),"Hide menu")){
            isMenu = false;
        }
    }

}
