using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BrAirDropUI : MonoBehaviour
{
    public UnityEvent OnNewAirDrop;
    public UnityEvent OnAirDropUnPacked;
    
    // Start is called before the first frame update
    void Awake()
    {
        BrAirdropController.Instance.OnNewAirDrop.AddListener(() =>
        {
            OnNewAirDrop.Invoke();
        });

        BrAirdropController.Instance.OnUnpack += player =>
        {
            OnAirDropUnPacked.Invoke();
        };
    }

}
