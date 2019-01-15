﻿using Photon.Pun;

class BrHealthPickup : BrPickupBase
{
    public int Health=50;
    protected override void GetReward(BrCharacterController currentPlayer)
    {
        base.GetReward(currentPlayer);
        currentPlayer.AddHealth(Health);
    }

    protected override bool CanPickup(BrCharacterController controller)
    {
        return (controller.NeedHealth);
    }


}