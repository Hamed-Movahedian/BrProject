using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class BrPlayerCamera : MonoBehaviour
{
    private BrCharacterController _characterController;
    private CinemachineVirtualCamera virtualCamera;

    private void OnEnable()
    {
        _characterController = GetComponentInParent<BrCharacterController>();
        virtualCamera = GetComponent<CinemachineVirtualCamera>();

        transform.SetParent(null);
        
        virtualCamera.Priority = 10;
        
        // active if only this is master player
        virtualCamera.gameObject.SetActive(_characterController.isMine);
        

        if (!_characterController.isMine)
            BrPlayerTracker.Instance.OnActivePlayerChange += (player, activePlayer) =>
            {
                if (player == _characterController)
                    gameObject.SetActive(false);
                
                if (activePlayer == _characterController)
                    gameObject.SetActive(true);
            };
        
        if(_characterController.isMine)
            _characterController.ParachuteState.OnLanding.AddListener((() =>
            {
                transform.position=BrFlyCutScene.Instance.VirtualCamera.transform.position;
                transform.rotation=BrFlyCutScene.Instance.VirtualCamera.transform.rotation;
            }));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
