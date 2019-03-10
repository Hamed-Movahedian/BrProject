using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BrFleeCondition
{
    public enum ConditionEnum
    {
        NoWeapon,
        HealthBelowThreshold,
        EnemyCounter,
        Always
    }

    public List<ConditionEnum> Conditions;
    
    [Range(0,1)]
    public float HealthThreshold=.3f;

    public int EnemyCounter=3;

    public bool IsValid(BrAiCharacterController controller)
    {

        foreach (var condition in Conditions)
        {
            switch (condition)
            {
                case ConditionEnum.HealthBelowThreshold:
                    if (controller.character.Health < controller.character.MaxHealth * HealthThreshold)
                        return true;
                    break;
                case ConditionEnum.EnemyCounter:
                    if (controller.playersInRange.Count >= EnemyCounter)
                        return true;
                    break;
                case ConditionEnum.Always:
                    return true;
                    break;
                case ConditionEnum.NoWeapon:
                    if (!controller.character.WeaponController.Armed)
                        return true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        return false;
    }
}