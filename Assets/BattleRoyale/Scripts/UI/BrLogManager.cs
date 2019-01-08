using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrLogManager : MonoBehaviour
{
    public static BrLogManager instance;
    public GameObject panle;
    public Text text;
    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    internal void LogKill(string kuserID1, string weaponName, string vuserID2)
    {
        text.text += kuserID1 + " " + weaponName + " " + vuserID2+"\n";
    }
}
