using System;
using System.Linq;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace BehaviorDesigner.Runtime.Tasks.BattleRoyale
{
    [TaskCategory("BattleRoyale")]
    public class Fire : BrAiTaskBase
    {
        public SharedGameObject Target;

        public float Interval = 2;

        public LayerMask LayerMask;

        private float lastTime = 0;

        public override TaskStatus OnUpdate()
        {
            if (Time.time - lastTime > Interval)
            {
                lastTime = Time.time;
                
                
                // set direction
                var dir = Target.Value.transform.position - characterController.transform.position;

                dir.y = 0;

                dir = dir.normalized;


                // set origin
                var origin = characterController.transform.position;

                origin.y = characterController.WeaponController.WeaponSlot.position.y;

                origin += dir * (characterController.CapsuleCollider.radius + 0.1f);

                
                // set range
                var range = .3f;

                var weapon = characterController.WeaponController.CurrWeapon;

                if (weapon)
                    range = weapon.BulletRange;

                
                // set aim
                characterController.AimVector = Vector3.zero;

                if (Physics.Raycast(origin, dir, out var hitInfo, range, LayerMask))
                    if (hitInfo.collider.gameObject == Target.Value)
                        characterController.AimVector = dir;
            }


            return TaskStatus.Running;
        }

        public override void OnEnd()
        {
            base.OnEnd();
            characterController.AimVector = Vector3.zero;

        }
    }
}