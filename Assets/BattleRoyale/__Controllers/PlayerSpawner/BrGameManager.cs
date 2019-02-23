using System;
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
        
        void Start()
        {

            if (playerPrefab == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
            }
            else
            {
                var position = JsonUtility.FromJson<Vector3>((string)PhotonNetwork.LocalPlayer.CustomProperties["Pos"]);
                
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
