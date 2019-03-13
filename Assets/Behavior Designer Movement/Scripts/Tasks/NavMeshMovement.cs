using UnityEngine;
using UnityEngine.AI;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    public abstract class NavMeshMovement : Movement
    {
        public SharedFloat arriveDistance = 0.2f;

        // Component references
        protected NavMeshAgent navMeshAgent;

        /// <summary>
        /// Cache the component references.
        /// </summary>
        public override void OnAwake()
        {
            navMeshAgent = gameObject.GetComponentInParent<NavMeshAgent>();
            
        }

        /// <summary>
        /// Allow pathfinding to resume.
        /// </summary>
        public override void OnStart()
        {
            if(navMeshAgent==null)
                navMeshAgent = gameObject.GetComponentInParent<NavMeshAgent>();
            
            navMeshAgent.isStopped = false;
        }

        /// <summary>
        /// Set a new pathfinding destination.
        /// </summary>
        /// <param name="destination">The destination to set.</param>
        /// <returns>True if the destination is valid.</returns>
        protected override bool SetDestination(Vector3 destination)
        {
            navMeshAgent.isStopped = false;
            return navMeshAgent.SetDestination(destination);
        }

        /// <summary>
        /// Specifies if the rotation should be updated.
        /// </summary>
        /// <param name="update">Should the rotation be updated?</param>
        protected override void UpdateRotation(bool update)
        {
            navMeshAgent.updateRotation = update;
            navMeshAgent.updateUpAxis = update;
        }

        /// <summary>
        /// Does the agent have a pathfinding path?
        /// </summary>
        /// <returns>True if the agent has a pathfinding path.</returns>
        protected override bool HasPath()
        {
            return navMeshAgent.hasPath && navMeshAgent.remainingDistance > arriveDistance.Value;
        }

        /// <summary>
        /// Returns the velocity of the agent.
        /// </summary>
        /// <returns>The velocity of the agent.</returns>
        protected override Vector3 Velocity()
        {
            return navMeshAgent.velocity;
        }

        /// <summary>
        /// Returns true if the position is a valid pathfinding position.
        /// </summary>
        /// <param name="position">The position to sample.</param>
        /// <returns>True if the position is a valid pathfinding position.</returns>
        protected bool SamplePosition(Vector3 position)
        {
            NavMeshHit hit;
            return NavMesh.SamplePosition(position, out hit, navMeshAgent.height * 5, NavMesh.AllAreas);
        }

        /// <summary>
        /// Has the agent arrived at the destination?
        /// </summary>
        /// <returns>True if the agent has arrived at the destination.</returns>
        protected override bool HasArrived()
        {
            // The path hasn't been computed yet if the path is pending.
            float remainingDistance;
            
            if (navMeshAgent.pathPending) {
                remainingDistance = float.PositiveInfinity;
            } else {
                remainingDistance = navMeshAgent.remainingDistance;
            }

            return remainingDistance <= arriveDistance.Value;
        }

        /// <summary>
        /// Stop pathfinding.
        /// </summary>
        protected override void Stop()
        {
            if (navMeshAgent.hasPath) {
                navMeshAgent.isStopped = true;
            }
        }

        /// <summary>
        /// The task has ended. Stop moving.
        /// </summary>
        public override void OnEnd()
        {
            
        }

        /// <summary>
        /// The behavior tree has ended. Stop moving.
        /// </summary>
        public override void OnBehaviorComplete()
        {
            Stop();
        }

        /// <summary>
        /// Reset the values back to their defaults.
        /// </summary>
        public override void OnReset()
        {
            arriveDistance = 1;
        }
    }
}