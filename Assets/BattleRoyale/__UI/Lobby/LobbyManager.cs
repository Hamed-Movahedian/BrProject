using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace BR.Lobby
{
    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        public static LobbyManager Instance;

        public GameObject MarkerPrefab;

        public RectTransform MarkerParent;

        private AsyncOperation asyncOperation;

        public UnityEvent OnCloseRoom;

        public bool ArenaLoading = true;

        private void Awake()
        {
            Instance = this;
        }

        // Use this for initialization
        void Start()
        {
            var marker = PhotonNetwork.Instantiate(this.MarkerPrefab.name, Vector3.zero, Quaternion.identity);
            Invoke(nameof(LoadAsync), 2);
        }

        private void LoadAsync()
        {
            if (!ArenaLoading)
                return;
            
            asyncOperation = SceneManager.LoadSceneAsync("Arena");
            asyncOperation.allowSceneActivation = false;
        }

        public void CloseRoom()
        {
            if (!PhotonNetwork.IsMasterClient)
                return;

            PhotonNetwork.CurrentRoom.IsOpen = false;

            photonView.RPC(nameof(CloseRoomRpc), RpcTarget.AllViaServer);
        }

        [PunRPC]
        public void CloseRoomRpc()
        {
            OnCloseRoom.Invoke();
        }

        public void LoadArena()
        {

            if (PhotonNetwork.IsMasterClient)
                photonView.RPC(nameof(LoadArenaRpc), RpcTarget.AllViaServer);
        }

        [PunRPC]
        private void LoadArenaRpc()
        {
            asyncOperation.allowSceneActivation = true;

            //PhotonNetwork.LoadLevel("Arena");
        }
    }
}