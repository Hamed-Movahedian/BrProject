using System.Collections;
using System.Collections.Generic;

public class BrChestPickup : BrPickupBase
{
    public List<BrChestPlaceHolder> PlaceHolders;
    public override void DisablePickup()
    {
        base.DisablePickup();
        
        PlaceHolders.ForEach(p=>p.Replace());
    }
}