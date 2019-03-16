using System;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class BrGameManager : MonoBehaviourPunCallbacks
{
    #region Instance

    private static BrGameManager _instance;

    public static BrGameManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<BrGameManager>();
            return _instance;
        }
    }

    #endregion

    #region OnStart

    public delegate void OnStartDel();

    public OnStartDel OnStart;

    #endregion

    public UnityEvent OnMatchFinished;

    private int arenaCount = 0;

    void Start()
    {
        photonView.RPC(nameof(StartGame), RpcTarget.All);
    }

    [PunRPC]
    private void StartGame()
    {
        arenaCount++;

        if (arenaCount < PhotonNetwork.CurrentRoom.PlayerCount)
            return;

        StartCoroutine(StartGameCorotine());
    }

    private IEnumerator StartGameCorotine()
    {
        yield return new WaitForSeconds(0.2f);
        
        OnStart?.Invoke();

    }

    public void FinishMatch()
    {
        ExitGame();
    }


    public void ExitGame()
    {
        OnMatchFinished.Invoke();
    }

    public void GotoMain()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("MainMenu");
    }
}