using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardMover : MonoBehaviour
{
    //public PlayerController Playercontroller;
    public void Update()
    {

        Vector3 dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        //Playercontroller.SetMovementSpeed(dir);
        
    }
}
