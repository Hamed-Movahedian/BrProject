using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class BrLineOfSight : MonoBehaviour
{
    public LineRenderer LineOfSight;
    public LayerMask CollitionMask;
    public BrCharacterController CharacterController;
    public Color TargetColor = Color.red;

    public BrWeaponController WeaponController;
    
    private Color defaultColor;


    void Start()
    {
        if (!CharacterController.IsMaster)
        {
            Destroy(gameObject);
            return;
        }

        LineOfSight.gameObject.SetActive(false);

        defaultColor = LineOfSight.material.GetColor("_TintColor");
    }

    void Update()
    {
        var weapon = WeaponController.CurrWeapon;
        if (CharacterController.AimVector.sqrMagnitude > 0 && weapon != null)
        {
            var start = weapon.FireLocation.position;
            Vector3 dir = WeaponController.transform.forward;
            dir.y = 0;
            var end = start + dir * weapon.BulletRange;

            RaycastHit hitInfo;

            if (Physics.Raycast(start, dir, out hitInfo, weapon.BulletRange, CollitionMask))
            {
                end = hitInfo.point;
                if (hitInfo.collider.CompareTag("Player"))
                    LineOfSight.material.SetColor("_TintColor", TargetColor);
                else
                    LineOfSight.material.SetColor("_TintColor", defaultColor);
            }
            else
            {
                LineOfSight.material.SetColor("_TintColor", defaultColor);
            }

            LineOfSight.SetPosition(0, start);
            LineOfSight.SetPosition(1, end);
            LineOfSight.gameObject.SetActive(true);
        }
        else
        {
            LineOfSight.gameObject.SetActive(false);
        }
    }
}