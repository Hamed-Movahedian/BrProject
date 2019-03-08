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
        public SharedGameObjectList Targets;
        private BrFleeTargertSelection targertSelection;
        private GameObject target;


        public override void OnAwake()
        {
            base.OnAwake();
            targertSelection = aiBehaviour.FleeTargertSelection;
        }

        public override TaskStatus OnUpdate()
        {
            switch (targertSelection.Method)
            {
                case BrFleeTargertSelection.MethodEnum.Closest:
                    SelectClosestTarget();
                    break;
                
                case BrFleeTargertSelection.MethodEnum.All:
                    SelectAllTargets();
                    break;
                
                case BrFleeTargertSelection.MethodEnum.Random:
                    SelectRandomTargets();
                    break;
                
                case BrFleeTargertSelection.MethodEnum.Weakest:
                    SelectWeakestTargets();
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            return Targets.Value.Count>0
                ? TaskStatus.Success
                : TaskStatus.Failure;
        }

        private void SelectWeakestTargets()
        {
            Targets.Value.Clear();
            Targets.Value.Add(
                aiController.playersInRange.Select(p=>(p.Health,p)).Min().Item2.gameObject);
        }

        private void SelectRandomTargets()
        {
            Targets.Value.Clear();
            Targets.Value.Add(aiController.playersInRange.RandomSelection().gameObject);
        }

        private void SelectAllTargets()
        {
            Targets.Value.Clear();
            Targets.Value.AddRange(aiController.playersInRange.Select(p => p.gameObject));
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

            Targets.Value.Clear();

            if (target != null)
                Targets.Value.Add(target);
        }
    }
}