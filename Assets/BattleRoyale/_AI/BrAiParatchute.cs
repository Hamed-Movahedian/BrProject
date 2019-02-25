using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrAiParatchute : MonoBehaviour
{
    private BrCharacterController player;
    private Vector3 destination;
    public float ArriveDistance=.1f;

    // Start is called before the first frame update
    void Start()
    {
        enabled = true;
        
        BrAIConteroller.Instance.OnInitialize += player =>
        {
            if (player == null)
            {
                enabled = false;
                return;
            }

            this.player = player;
            
            // enable on para open
            player.ParachuteState.OnOpenPara.AddListener(() => enabled = true);
            
            // disable on landing
            player.ParachuteState.OnLanding.AddListener(() => enabled = false);
            
            // Set destination
            BrAIConteroller.Instance.OnSetDestination += pos =>
            {
                destination = pos;
            };

        };
    }

    // Update is called once per frame
    void Update()
    {
        var dir = player.transform.position-destination;

        dir.y = 0;
        
        if(Vector3.SqrMagnitude(dir)<ArriveDistance*ArriveDistance)
            dir=Vector3.zero;

        player.MovVector = dir.normalized;

    }
}
