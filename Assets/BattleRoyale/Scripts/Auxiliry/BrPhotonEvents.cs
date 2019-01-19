using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BrPhotonEvents : MonoBehaviour
{
    public UnityEvent OnMaster;
    public UnityEvent OnNotMaster;
    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
            OnMaster.Invoke();
        else
            OnNotMaster.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
