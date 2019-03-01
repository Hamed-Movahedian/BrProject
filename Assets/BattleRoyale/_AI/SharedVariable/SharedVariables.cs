using UnityEngine;

namespace BehaviorDesigner.Runtime
{
    [System.Serializable]
    public class SharedPickup : SharedVariable<BrPickupBase>
    {
        public static implicit operator SharedPickup(BrPickupBase value)
        {
            return new SharedPickup { mValue = value };
        }
    }
    
    [System.Serializable]
    public class SharedWeaponPickup : SharedVariable<BrWeaponPickup>
    {
        public static implicit operator SharedWeaponPickup(BrWeaponPickup value)
        {
            return new SharedWeaponPickup { mValue = value };
        }
    }
    
    [System.Serializable]
    public class SharedHealthPickup : SharedVariable<BrHealthPickup>
    {
        public static implicit operator SharedHealthPickup(BrHealthPickup value)
        {
            return new SharedHealthPickup { mValue = value };
        }
    }
    
    [System.Serializable]
    public class SharedShieldPickup : SharedVariable<BrShieldPickup>
    {
        public static implicit operator SharedShieldPickup(BrShieldPickup value)
        {
            return new SharedShieldPickup { mValue = value };
        }
    }
    
    [System.Serializable]
    public class SharedAmmoPickup : SharedVariable<BrAmmoPickup>
    {
        public static implicit operator SharedAmmoPickup(BrAmmoPickup value)
        {
            return new SharedAmmoPickup { mValue = value };
        }
    }
    
    [System.Serializable]
    public class SharedCharacter : SharedVariable<BrCharacterController>
    {
        public static implicit operator SharedCharacter(BrCharacterController value)
        {
            return new SharedCharacter { mValue = value };
        }
    }
    
    [System.Serializable]
    public class SharedChest : SharedVariable<BrChestPickup>
    {
        public static implicit operator SharedChest(BrChestPickup value)
        {
            return new SharedChest { mValue = value };
        }
    }
    
    
}
