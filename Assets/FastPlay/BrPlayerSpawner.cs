using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrPlayerSpawner : MonoBehaviour
{
    public int ActorNumber = 0;

    private void Awake()
    {
        BrPlayerTracker.Instance.OnPlayerRegisterd += player =>
        {
            
            if (player.photonView.Controller.ActorNumber == ActorNumber)
            {
                var position = transform.position;
                position.y = player.transform.position.y;
                player.transform.position = position;
            }
        };
    }

    private void OnValidate()
    {
        gameObject.name = "Player " + ActorNumber;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position,.5f);
    }
}
