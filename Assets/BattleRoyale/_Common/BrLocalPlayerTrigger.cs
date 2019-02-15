using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BrLocalPlayerTrigger : MonoBehaviour
{
    public UnityEvent OnEnter;
    public UnityEvent OnExit;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) 
            return;
        
        var characterController = other.GetComponent<BrCharacterController>();
            
        if(characterController && characterController.isMine)
            OnEnter.Invoke();
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) 
            return;
        
        var characterController = other.GetComponent<BrCharacterController>();
            
        if(characterController && characterController.isMine)
            OnExit.Invoke();
    }
}
