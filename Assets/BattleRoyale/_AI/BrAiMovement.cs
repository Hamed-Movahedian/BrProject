using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BrAiMovement : MonoBehaviour
{
    public float BreakDistance = 3;
    private NavMeshAgent navMeshAgent;
    private BrCharacterController characterController;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponentInParent<BrCharacterController>();
        navMeshAgent = GetComponentInParent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir;

        bool b = true;
        switch (characterController.CurrentState)
        {
            case CharacterStateEnum.Falling:
            case CharacterStateEnum.Parachute:
                dir = navMeshAgent.destination - transform.position;
                break;
            default:
                dir = navMeshAgent.steeringTarget - transform.position;

                if (navMeshAgent.steeringTarget != navMeshAgent.destination)
                    b = false;

                break;
        }

        dir.y = 0;

        if (b)
            dir = dir / BreakDistance;

        
        if (dir.sqrMagnitude > 1 || !b)
            dir = dir.normalized;

        characterController.MovVector = dir;
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying || navMeshAgent.updatePosition == false)
            return;

        Gizmos.color = Color.yellow;
        var path = navMeshAgent.path;
        for (int i = 0; i < path.corners.Length; i++)
        {
            if (i > 0)
                Gizmos.DrawLine(path.corners[i - 1], path.corners[i]);

            Gizmos.DrawSphere(path.corners[i], .3f);
        }
    }
}