using System;
using UnityEngine;

[CreateAssetMenu(menuName = "BattleRoyale/AIBehaviour")]
public class BrAiBehavioursAsset : ScriptableObject
{
    #region Pickups
    [Header("Pickups")]
    public BrAiHealthPickup HealthPickup;
    public BrAiShieldPickup ShieldPickup;
    public BrAiAmmoPickup AmmoPickup;
    public BrAiWeaponPickup WeaponPickup;

    #endregion

    [Header("Flee")]
    public BrFleeCondition FleeCondition;

    public BrFleeTargertSelection FleeTargertSelection;
}

#region Pickup classes

[Serializable]
public class BrAiHealthPickup
{
    [Range(0, 1)] public float Threshold;
}

[Serializable]
public class BrAiShieldPickup
{
    [Range(0, 1)] public float Threshold;
}

[Serializable]
public class BrAiAmmoPickup
{
    [Range(0, 1)] public float Threshold;
}

[Serializable]
public class BrAiWeaponPickup
{
    public bool HasWeapon;
}

#endregion