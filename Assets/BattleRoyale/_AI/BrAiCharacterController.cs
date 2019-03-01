using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;

public class BrAiCharacterController : MonoBehaviour
{
    private BrCharacterController characterController;
    private BehaviorTree behaviorTree;

    // Start is called before the first frame update
    void Awake()
    {
        if (!BrAIConteroller.Instance.IsActive)
        {
            gameObject.SetActive(false);
            return;
        }
        characterController = GetComponentInParent<BrCharacterController>();

        behaviorTree = GetComponent<BehaviorTree>();

        characterController.ParachuteState.OnLanding.AddListener(()=>
        {
            Invoke(nameof(StartTree),0.1f);//,Random.Range(0.5f,1));
        });
        
    }

    private void StartTree()
    {
        behaviorTree.EnableBehavior();
    }
}
