using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class BrWeaponPickup : BrPickupBase
{
    public string WeaponName;

    protected override void GetReward(BrCharacterController player)
    {
        base.GetReward(player);

        player.WeaponController.PickupWeapon(WeaponName);
    }

    [PunRPC]

    protected override void DisablePickup()
    {
        base.DisablePickup();
    }
}
