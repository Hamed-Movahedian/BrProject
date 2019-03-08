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
    void Awake()
    {
        if (!BrAIConteroller.Instance.IsActive)
        {
            gameObject.SetActive(false);
            return;
        }
 
        character.ParachuteState.OnLanding.AddListener(()=>
        {
            Invoke(nameof(StartTree),0.1f);//,Random.Range(0.5f,1));
        });
        
    }

    private void StartTree()
    {
        behaviorTree.EnableBehavior();
    }
}
