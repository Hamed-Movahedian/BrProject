using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class BrLineOfSight : MonoBehaviour
{
    public LineRenderer LineOfSight;
    public LayerMask CollitionMask;
    public PhotonView PhotonView;
    public Color TargetColor = Color.red;

    public BrWeaponController WeaponController;
    private Color defaultColor;

    // Start is called before the first frame update
    void Start()
    {
        if ( PhotonNetwork.LocalPlayer.CustomProperties["AI"].ToString() == "1")
        {
            gameObject.SetActive(false);
            return;
        }
        defaultColor=LineOfSight.material.GetColor("_TintColor");

        if (PhotonView.IsMine)
            LineOfSight.gameObject.SetActive(false);
        else
            Destroy(gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        var weapon = WeaponController.CurrWeapon;
        if (BrJoystickController.Instance.AimJoystick.Value3.sqrMagnitude > 0 && weapon!=null)
        {
            var start = weapon.FireLocation.position;
            Vector3 dir = WeaponController.transform.forward;
            dir.y = 0;
            var end = start + dir * weapon.BulletRange;

            RaycastHit hitInfo;

            if (Physics.Raycast(start, dir, out hitInfo, weapon.BulletRange, CollitionMask))
            {

                end = hitInfo.point;
                if(hitInfo.collider.CompareTag("Player"))
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
