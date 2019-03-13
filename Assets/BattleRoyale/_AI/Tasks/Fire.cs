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

        private Vector3 lastTargetPos;

        public override void OnStart()
        {
            base.OnStart();
            lastTargetPos = Target.Value.transform.position;
        }

        public override TaskStatus OnUpdate()
        {
            if (Time.time - lastTime > Interval)
            {
                lastTime = Time.time;
                
                var weapon = characterController.WeaponController.CurrWeapon;

                // set direction
                var dir = Target.Value.transform.position - characterController.transform.position;

                dir.y = 0;

                var bulletTime = dir.magnitude / (weapon.BulletPrefab.Speed);
 
                var targetSpeed = (Target.Value.transform.position - lastTargetPos) / Interval;
                
                Debug.Log($"targetSpeed={targetSpeed}");
                Debug.Log($"bulletTime={bulletTime}");
                var pPos = Target.Value.transform.position+ targetSpeed*5* bulletTime;
                
                Debug.DrawLine(Target.Value.transform.position,pPos,Color.red,1);
                Debug.DrawLine(characterController.transform.position,pPos,Color.green,1);

                var pDir = pPos-characterController.transform.position;
                
                var aim=Vector3.Lerp(dir,pDir,aiBehaviour.AimBehaviour.predictionAccuracy);
                
                Debug.Log($"aiBehaviour.AimBehaviour.predictionAccuracy={aiBehaviour.AimBehaviour.predictionAccuracy}");


                // set origin
                var origin = characterController.transform.position;

                origin.y = characterController.WeaponController.WeaponSlot.position.y;

                
                // set range
                var range = .3f;


                if (weapon)
                    range = weapon.BulletRange;

                
                // set aim
                characterController.AimVector = Vector3.zero;

                if (Physics.Raycast(origin, dir, out var hitInfo, range, LayerMask))
                    if (hitInfo.collider.gameObject == Target.Value)
                        characterController.AimVector = aim.normalized;

                
                lastTargetPos = Target.Value.transform.position;
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