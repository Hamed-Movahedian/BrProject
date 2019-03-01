using Photon.Pun;

public class BrHealthPickup : BrPickupBase
{
    public int Health=50;
    protected override void GetReward(BrCharacterController currentPlayer)
    {
        base.GetReward(currentPlayer);
        currentPlayer.Health+=Health;
    }

    protected override bool CanPickup(BrCharacterController controller)
    {
        return (controller.NeedHealth);
    }


}