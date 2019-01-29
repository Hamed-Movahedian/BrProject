using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class BrFlyCutScene : MonoBehaviour
{
    #region Instance

    private static BrFlyCutScene _instance;

    public static BrFlyCutScene Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<BrFlyCutScene>();
            return _instance;
        }
    }

    #endregion

    public PlayableDirector Director;
    public BrCharacterModel CutScenePlayer;
    public CinemachineVirtualCamera VirtualCamera;
    
    [Header("Events")]
    public UnityEvent OnStartFalling;
    public UnityEvent OnOpenPara;
    public UnityEvent OnLanding;
    
    private BrCharacterController _player;

    // ********************************** methods

    private void Awake()
    {
        // Master player register
        BrPlayerTracker.Instance.OnMasterPlayerRegister += player =>
        {
            _player = player;

            CutScenePlayer.SetProfile(_player.profile);
            
            // set initial pos

            StartCutScene();
            SetTransforms();
        };
        
    }

    private void SetTransforms()
    {
        var position = _player.transform.position;
        position.y = transform.position.y;
        transform.position = position;

        position.y = CutScenePlayer.transform.position.y;
        // set player transform;
        _player.transform.position = position;
        //_player.transform.rotation = CutScenePlayer.transform.rotation;
    }

    private void Update()
    {
        if(_player)
            SetTransforms();
    }

    public void StartCutScene()
    {
        Director.Play();
        OnStartFalling.Invoke();
    }
    
    public void OpenPara()
    {
        if (!Application.isPlaying)
            return;
        
        OnOpenPara.Invoke();
        _player.OpenPara();
        CutScenePlayer.gameObject.SetActive(false);
    }

    public void Land()
    {
        if (!Application.isPlaying)
            return;
        
        OnLanding.Invoke();
        _player.Land();
        _player = null;
        gameObject.SetActive(false);
    }
}
