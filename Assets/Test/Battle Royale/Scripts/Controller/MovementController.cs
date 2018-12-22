using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MovementController : BrJoystick
{
    public override void CalculateDrag()
    {
        base.CalculateDrag();

        Direction *= .01f;
        if (Direction.magnitude > 1)
            Direction = Direction.normalized;
    }

    public void Update()
    {
        Vector3 dir = new Vector3(Direction.x, 0, Direction.y);
        //PlayerController.SetMovementSpeed(dir);
    }

}
