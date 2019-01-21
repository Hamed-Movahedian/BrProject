using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrSimpleRotator : MonoBehaviour
{
    public float speed=90;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.RotateAround(Vector3.up, speed * Time.deltaTime);
	}
}
