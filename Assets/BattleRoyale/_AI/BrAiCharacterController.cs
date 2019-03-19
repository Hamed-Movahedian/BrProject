using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;

public class BrAiCharacterController : MonoBehaviour
{
    public BrCharacterController character;
    public Transform CharacterTransform;
    public BrAiBehavioursAsset aiBehaviour;
    public BehaviorTree behaviorTree;
    
    [NonSerialized]
    public List<BrCharacterController> playersInRange=new List<BrCharacterController>();


    // Start is called before the first frame update
    void Start()
    {
        if (!character.IsAi)
        {
            Destroy(gameObject);
            return;
        }

        behaviorTree.SetVariable("AiCharacterController",new SharedAiCharacterController {Value = this});
        
        character.ParachuteState.OnLanding.AddListener(()=>
        {
            Invoke(nameof(StartTree),0.1f);
        });
        
    }

    private void StartTree()
    {
        behaviorTree.EnableBehavior();
    }


}
