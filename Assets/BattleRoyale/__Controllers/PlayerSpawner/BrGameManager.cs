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

    public GameObject playerPrefab;

    public UnityEvent OnMatchFinished;

    private int arenaCount=0;

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
        
        if (playerPrefab == null)
        {
            Debug.LogError(
                "<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'",
                this);
        }
        else
        {
            var position = JsonUtility.FromJson<Vector3>((string) PhotonNetwork.LocalPlayer.CustomProperties["Pos"]);

            position = BrLevelBound.Instance.GetLevelPos(position);

            PhotonNetwork.Instantiate(this.playerPrefab.name, position, Quaternion.identity, 0);
        }
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