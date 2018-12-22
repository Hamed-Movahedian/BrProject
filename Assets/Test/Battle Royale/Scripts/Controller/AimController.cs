using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AimController : BrJoystick
{
    public float ShotRate = .9f;
    public Color ShotColor=Color.red;

    private Color _defaultColor;
    private bool _isAiming;

    public override void Start()
    {
        base.Start();
        _defaultColor = JoyesticImage.color;
    }

    public override void OnBeginDrag(PointerEventData data)
    {
        base.OnBeginDrag(data);
        _isAiming = true;
    }

    public override void CalculateDrag()
    {
        base.CalculateDrag();

        if (Direction.magnitude > Screen.height*0.18f*ShotRate)
        {
            JoyesticImage.color = ShotColor;
            Debug.Log("!!!! Shot !!!!");
        }
        else
            JoyesticImage.color = _defaultColor;

        Direction = Direction.normalized;
    }

    public override void OnEndDrag(PointerEventData data)
    {
        base.OnEndDrag(data);
        _isAiming = false;
        //PlayerController.SetAim(false, Vector3.zero);

    }

    public void Update()
    {
        Vector3 dir = new Vector3(Direction.x, 0, Direction.y);
        //PlayerController.SetAim(_isAiming, dir);

    }
}
