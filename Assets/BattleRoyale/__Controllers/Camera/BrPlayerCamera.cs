using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class BrPlayerCamera : MonoBehaviour
{
    private BrCharacterController _characterController;
    private CinemachineVirtualCamera virtualCamera;

    // Start is called before the first frame update
    void Start()
    {
        _characterController = GetComponentInParent<BrCharacterController>();
        virtualCamera = GetComponent<CinemachineVirtualCamera>();

        virtualCamera.Priority = _characterController.isMine ? 2 : 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
