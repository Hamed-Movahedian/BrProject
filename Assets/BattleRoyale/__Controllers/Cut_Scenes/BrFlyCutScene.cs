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

    private int playerCount;
    // ********************************** methods

    private void Awake()
    {
        BrPlayerSpawner.Instance.OnPlayerSpawned += player =>
        {
            playerCount++;
        };
        // Master player register
        BrPlayerTracker.Instance.OnPlayerRegisterd += player =>
        {
            if (!player.isMine)
                return;

            players.Add(player);

            if (player.IsMaster)
            {
                CutScenePlayer.SetProfile(player.profile);
                
                
                transform.position=player.transform.position;
            }

            // set initial pos

            if(players.Count<playerCount)
                return;
            
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

        
    }

    private void Update()
    {
        if (players.Count==playerCount)
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