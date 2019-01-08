using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace BR.Lobby
{
    public class LobbyCounter : MonoBehaviourPunCallbacks,IPunObservable
    {
        public Text Text;

        public int MaxPlayer = 2;
        public int Counter = 3;
        // Use this for initialization
        void Start ()
        {
            Text.text = "";
            check();
        }

        // Update is called once per frame
        void Update ()
        {
		
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            //if (!PhotonNetwork.IsMasterClient)
            //    return;
            check();

        }

        private void check()
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == MaxPlayer)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                StartCoroutine(Count());
            }
        }

        private IEnumerator Count()
        {
            for (int i = 0; i < Counter; i++)
            {
                Text.text = i.ToString();
                yield return new WaitForSeconds(1);
            }
            PhotonNetwork.LoadLevel("Arena");
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(Text.text);
            }
            else
            {
                Text.text = (string) stream.ReceiveNext();
            }
        }
    }
}
