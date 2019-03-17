using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class BrPlayerCamera : MonoBehaviour
{
    private BrCharacterController _characterController = null;
    private CinemachineVirtualCamera virtualCamera;


    private void OnEnable()
    {
        if (_characterController)
            return;

        _characterController = GetComponentInParent<BrCharacterController>();
        virtualCamera = GetComponent<CinemachineVirtualCamera>();

        transform.SetParent(null);

        virtualCamera.Priority = 10;

        // active if only this is master player
        virtualCamera.gameObject.SetActive(_characterController.IsMaster);


        BrPlayerTracker.Instance.OnActivePlayerChange += (player, activePlayer) =>
        {
            if (player == _characterController)
                gameObject.SetActive(false);

            if (activePlayer == _characterController)
            {
                gameObject.SetActive(true);
            }
        };

        _characterController.ParachuteState.OnLanding.AddListener((() =>
        {
            if (_characterController.IsMaster)
                transform.position = BrFlyCutScene.Instance.VirtualCamera.transform.position;
            
            transform.rotation = BrFlyCutScene.Instance.VirtualCamera.transform.rotation;
        }));
    }
}