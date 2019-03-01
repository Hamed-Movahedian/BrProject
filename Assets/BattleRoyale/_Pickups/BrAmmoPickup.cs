using Photon.Pun;

public class BrAmmoPickup : BrPickupBase
{
    public int BulletCount=50;
    protected override void GetReward(BrCharacterController currentPlayer)
    {
        base.GetReward(currentPlayer);
        currentPlayer.WeaponController.PickupAmmo(BulletCount);
    }

    protected override bool CanPickup(BrCharacterController controller)
    {
        return (controller.WeaponController.CanPickupAmmo());
    }



}