using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    [TaskDescription("Flee from the target specified using the Unity NavMesh.")]
    [TaskCategory("Movement")]
    [HelpURL("https://www.opsive.com/support/documentation/behavior-designer-movement-pack/")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}FleeIcon.png")]
    public class FleeAdvance : NavMeshMovement
    {        
        [Tooltip("The distance to look ahead when fleeing")]
        public SharedFloat lookAheadDistance = 5;

        private bool hasMoved;
        private BrAiCharacterController aiController;

        public override void OnAwake()
        {
            aiController = ((SharedAiCharacterController) Owner.GetVariable("AiCharacterController")).Value;
        }
        public override void OnStart()
        {
            base.OnStart();

            hasMoved = false;

            SetDestination(Target());
        }

        // Flee from the target. Return success once the agent has fleed the target by moving far enough away from it
        // Return running if the agent is still fleeing
        public override TaskStatus OnUpdate()
        {
            if (HasArrived()) {
                /*if (!hasMoved) {
                    return TaskStatus.Failure;
                }*/
                
                if (!SetDestination(Target())) {
                    return TaskStatus.Failure;
                }
                hasMoved = false;
            } else {
                // If the agent is stuck the task shouldn't continue to return a status of running.
                var velocityMagnitude = Velocity().sqrMagnitude;
                if (hasMoved && velocityMagnitude <= 0f) {
                    return TaskStatus.Failure;
                }
                hasMoved = velocityMagnitude > 0f;
            }

            return TaskStatus.Running;
        }

        // Flee in the opposite direction
        private Vector3 Target()
        {
            var dir = Vector3.zero;
            
            foreach (var player in aiController.playersInRange)
            {
                var v = transform.position - player.transform.position;
                
                var magnitude = Mathf.Max(0.0001f,v.sqrMagnitude);
                
                v = v.normalized * (1 / magnitude);

                dir += v;
            }

            return transform.position + dir.normalized * lookAheadDistance.Value;
        }

        // Return false if the position isn't valid on the NavMesh.
        protected override bool SetDestination(Vector3 destination)
        {
            if (!SamplePosition(destination)) {
                return false;
            }
            return base.SetDestination(destination);
        }

        // Reset the public variables
        public override void OnReset()
        {
            base.OnReset();
            lookAheadDistance = 5;
        }
    }
}