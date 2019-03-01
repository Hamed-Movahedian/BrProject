using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.BattleRoyale
{
    [TaskCategory("BattleRoyale")]
    //[TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}WithinDistanceIcon.png")]

    public class ObjectInRange : Conditional
    {
        public float distance = 18;
        public float updateDistance = 1;
        public LayerMask layer;
        public SharedGameObject result = null;

        private bool dirty = true;
        private Vector3 lastPos;

        public override void OnStart()
        {
        }

        public override TaskStatus OnUpdate()
        {
            if (transform == null)
                return TaskStatus.Failure;

            var pos = transform.position;
            
            pos.y = BrLevelBound.Instance.Y;
            
            if (!dirty && (lastPos - pos).sqrMagnitude > updateDistance * updateDistance)
                dirty = true;

            if (dirty)
            {
                result.Value = null;
                dirty = false;
                lastPos = pos;

                var colliders =
                    Physics.OverlapSphere(pos, distance, layer, QueryTriggerInteraction.Collide);

                var min = distance * distance + 10;

                foreach (var collider in colliders)
                {
                    var delta = collider.transform.position - pos;
                    delta.y = 0;
                    var sqrMagnitude = delta.sqrMagnitude;
                    if (sqrMagnitude < min)
                    {
                        min = sqrMagnitude;
                        result.Value = collider.gameObject;
                    }
                }

            }

            return (result.Value == null || !result.Value.activeSelf)  ? TaskStatus.Failure : TaskStatus.Success;
        }
    }
}