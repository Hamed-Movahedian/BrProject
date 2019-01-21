using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrCharacterShadow : MonoBehaviour
{
    public BrCharacterController Controller;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	    var distance = Controller.GroundDistance;
	    //if(distance>1)
	    transform.position = Controller.GroundHitInfo.point;
        transform.LookAt(transform.position+Controller.GroundHitInfo.normal);
	}
}
