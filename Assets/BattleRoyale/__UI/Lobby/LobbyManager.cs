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

        // Use this for initialization
        void Start()
        {
            Instance = this;

            var marker = PhotonNetwork.Instantiate(this.MarkerPrefab.name, Vector3.zero, Quaternion.identity);
        }

        public void CloseRoom()
        {
            if (!PhotonNetwork.IsMasterClient)
                return;

            PhotonNetwork.CurrentRoom.IsOpen = false;

            photonView.RPC(nameof(CloseRoomRpc),RpcTarget.AllViaServer);
            
        }

        [PunRPC]
        public void CloseRoomRpc()
        {
            asyncOperation = SceneManager.LoadSceneAsync("Arena");
            asyncOperation.allowSceneActivation = false;
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