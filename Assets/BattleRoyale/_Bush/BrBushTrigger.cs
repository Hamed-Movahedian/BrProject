using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrBushTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (BrBushModule.Modules.TryGetValue(other, out var module))
            {
                module.Enter();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (BrBushModule.Modules.TryGetValue(other, out var module))
            {
                module.Exit();
            }
        }
    }
}
