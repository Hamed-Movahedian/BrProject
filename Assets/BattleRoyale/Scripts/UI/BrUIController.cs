using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrUIController : MonoBehaviour
{
    public static BrUIController Instance;

    public BrJoystick MovementJoystick;
    public BrJoystick AimJoystick;
    private void Awake()
    {
        Instance = this;
    }


    // Use this for initialization
    void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetMovementJoyisticActive(bool b)
    {
        MovementJoystick.gameObject.SetActive(b);
    }
    public void SetAimJoyisticActive(bool b)
    {
        AimJoystick.gameObject.SetActive(b);
    }
}
