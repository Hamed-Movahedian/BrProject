using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrShieldPickup : BrPickupBase
{
    public int Sheild = 50;
    protected override void GetReward(BrCharacterController currentPlayer)
    {
        base.GetReward(currentPlayer);
        currentPlayer.AddShield(Sheild);
    }
    protected override bool CanPickup(BrCharacterController controller)
    {
        return (controller.NeedShield);
    }
}
