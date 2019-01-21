using Photon.Pun;
using UnityEngine;

namespace BR.Lobby
{
    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        public static LobbyManager Instance;

        public GameObject MarkerPrefab;

        public RectTransform MarkerParent;
        // Use this for initialization
        void Start ()
        {
            Instance = this;

            var marker = PhotonNetwork.Instantiate(this.MarkerPrefab.name,Vector3.zero, Quaternion.identity);
        }

        // Update is called once per frame
        void Update () {
		
        }
    }
}
