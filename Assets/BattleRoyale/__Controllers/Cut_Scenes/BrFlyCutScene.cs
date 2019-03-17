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

    [Header("Events")] public UnityEvent OnStartFalling;
    public UnityEvent OnOpenPara;
    public UnityEvent OnLanding;

    private List<BrCharacterController> players = new List<BrCharacterController>();

    // ********************************** methods

    private void Awake()
    {
        // Master player register
        BrPlayerTracker.Instance.OnPlayerRegisterd += player =>
        {
            if (!player.isMine)
                return;

            players.Add(player);

            if (player.IsMaster)
                CutScenePlayer.SetProfile(player.profile);

            // set initial pos

            SetTransforms();
            StartCutScene();
        };
    }

    private void SetTransforms()
    {
        foreach (var player in players)
        {
            var position = player.transform.position;

            position.y = CutScenePlayer.transform.position.y;

            player.transform.position = position;
        }

        transform.position = BrCharacterController.MasterCharacter.transform.position;
    }

    private void Update()
    {
        if (players.Count > 0)
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

        players.ForEach(p => p.OpenPara());

        CutScenePlayer.gameObject.SetActive(false);
    }

    public void Land()
    {
        if (!Application.isPlaying)
            return;

        OnLanding.Invoke();
        players.ForEach(p => p.Land());
        players = null;
        gameObject.SetActive(false);
    }
}