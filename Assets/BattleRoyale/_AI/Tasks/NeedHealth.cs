using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace BehaviorDesigner.Runtime.Tasks.BattleRoyale
{
    [TaskCategory("BattleRoyale")]
    public class NeedHealth : BrAiConditionalBase
    {
        private BrAiHealthPickup healthPickup;

        public override void OnAwake()
        {
            base.OnAwake();
            healthPickup = aiBehaviour.HealthPickup;
        }

        public override TaskStatus OnUpdate()
        {
            return characterController.Health < characterController.MaxHealth * healthPickup.Threshold
                ? TaskStatus.Success
                : TaskStatus.Failure;
        }
    }
}