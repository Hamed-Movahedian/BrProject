using System;
using System.Linq;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace BehaviorDesigner.Runtime.Tasks.BattleRoyale
{
    [TaskCategory("BattleRoyale")]
    public class TargetSelection : BrAiConditionalBase
    {
        public SharedGameObject Target;
        
        
        private GameObject target;


        public override TaskStatus OnUpdate()
        {
            Target.Value = null;
            
            switch (aiBehaviour.attackTargetSelection.Method)
            {
                case BrTargetSelection.MethodEnum.Closest:
                    SelectClosestTarget();
                    break;
                
                case BrTargetSelection.MethodEnum.Random:
                    SelectRandomTargets();
                    break;
                
                case BrTargetSelection.MethodEnum.Weakest:
                    SelectWeakestTargets();
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            return Target.Value!=null
                ? TaskStatus.Success
                : TaskStatus.Failure;
        }

        private void SelectWeakestTargets()
        {
            
            if(aiController.playersInRange.Count>0)
                Target.Value=aiController.playersInRange.Select(p=>(p.Health,p)).Min().Item2.gameObject;
        }

        private void SelectRandomTargets()
        {
            if(aiController.playersInRange.Count>0)
            Target.Value=aiController.playersInRange.RandomSelection().gameObject;
        }

        private void SelectClosestTarget()
        {
            float min = float.MaxValue;
            
            target = null;
            
            foreach (var player in aiController.playersInRange)
            {
                var sqrMagnitude = (player.transform.position - transform.position).sqrMagnitude;
                
                if (sqrMagnitude < min)
                {
                    min = sqrMagnitude;
                    target = player.gameObject;
                }
            }

            Target.Value=target;
        }
    }
}