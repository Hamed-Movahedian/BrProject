using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace BehaviorDesigner.Runtime.Tasks.BattleRoyale
{
    [TaskCategory("BattleRoyale")]
    public class PlayerInRange : BrAiConditionalBase
    {
        public float UpdateIntervals = 2;
        public float distance = 18;
        public LayerMask layer;

        private float lastUpdateTime = 0;
        private List<BrCharacterController> playersInRange;
        private Collider[] colliders = new Collider[40];


        public override void OnAwake()
        {
            base.OnAwake();
            playersInRange = aiController.playersInRange;
        }

        public override TaskStatus OnUpdate()
        {
            if (Time.time - lastUpdateTime >= UpdateIntervals)
            {
                playersInRange.Clear();


                var size = Physics.OverlapSphereNonAlloc(transform.position, distance, colliders, layer,
                    QueryTriggerInteraction.Collide);

                lastUpdateTime = Time.time;

                for (var i = 0; i < size; i++)
                {
                    var character = BrCharacterDictionary.Instance.GetCharacter(colliders[i]);
                    
                    if (character && character != characterController)
                        playersInRange.Add(character);
                }
            }

            return playersInRange.Count > 0 ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}