using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BR
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        public static GameManager Instance;
    
        [Tooltip("The prefab to use for representing the player")]
        public GameObject playerPrefab;
        void Start()
        {

            if (playerPrefab == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
            }
            else
            {
                    PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
            }
        }
    }
}
