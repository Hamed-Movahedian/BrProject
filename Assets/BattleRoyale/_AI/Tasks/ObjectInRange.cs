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
        private Collider[] colliders = new Collider[40];

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


                var size = Physics.OverlapSphereNonAlloc(pos, distance, colliders, layer,
                    QueryTriggerInteraction.Collide);

                var min = distance * distance + 10;

                for (var i = 0; i < size; i++)
                {
                    var collider = colliders[i];

                    if(BrKillZone.Instance.IsInDanger(collider.transform.position))
                        continue;
                    
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

            return (result.Value == null || !result.Value.activeSelf) ? TaskStatus.Failure : TaskStatus.Success;
        }
    }
}