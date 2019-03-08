using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace BehaviorDesigner.Runtime.Tasks.BattleRoyale
{
    [TaskCategory("BattleRoyale")]
    public class MustFlee : BrAiConditionalBase
    {
        private BrFleeCondition condition;

        public override void OnAwake()
        {
            base.OnAwake();
            condition = aiBehaviour.FleeCondition;
        }

        public override TaskStatus OnUpdate()
        {
            return condition.IsValid(aiController)
                ? TaskStatus.Success
                : TaskStatus.Failure;
        }
    }
}