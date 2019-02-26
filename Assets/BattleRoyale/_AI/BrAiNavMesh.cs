using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BrAiNavMesh : MonoBehaviour
{
    public NavMeshAgent NavMeshAgent;

    private BrCharacterController player;
    private Vector3 destination = Vector3.zero;
    public float ArriveDistance = .3f;
    private Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        enabled = false;

        BrAIConteroller.Instance.OnInitialize += player =>
        {
            if (player == null)
            {
                gameObject.SetActive(false);
                return;
            }

            this.player = player;

            // Activate on landing
            player.ParachuteState.OnLanding.AddListener(() =>
            {
                enabled = true;
                NavMeshAgent.Warp(player.transform.position);
                transform.position = player.transform.position = NavMeshAgent.nextPosition;
                transform.SetParent(player.transform);

                if (destination != Vector3.zero)
                    NavMeshAgent.SetDestination(destination);
            });

            // Disable on dead!
            player.OnDead.AddListener(() => { enabled = false; });

            // Set destination
            BrAIConteroller.Instance.OnSetDestination += pos =>
            {
                destination = pos;

                if (enabled)
                    NavMeshAgent.SetDestination(destination);
            };
        };
    }

    // Update is called once per frame
    void Update()
    {
        var dir = player.transform.position - NavMeshAgent.destination;

        dir.y = 0;

        var arriveD = Vector3.SqrMagnitude(dir);
        
        if (arriveD < ArriveDistance * ArriveDistance)
        {
            dir= Vector3.zero;
        }
        else
        {
            dir = NavMeshAgent.steeringTarget - player.transform.position;

            dir.y = 0;
        }

        //player.MovVector = arriveD > 1 ? dir.normalized : dir;
        player.MovVector = Vector3.SmoothDamp(player.MovVector,dir,ref velocity,0.3f);
    }
}