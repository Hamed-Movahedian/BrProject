using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace BR.Lobby
{
    public class PlayerMarker : MonoBehaviourPunCallbacks, IPunObservable
    {
        public List<Image> Markers;

        // Use this for initialization
        void Start ()
        {
            Markers.ForEach(m=>m.color = JsonUtility.FromJson<Color>((string) photonView.Owner.CustomProperties["Color"]));
            var rectTransform = GetComponent<RectTransform>();
            
            rectTransform.SetParent(LobbyManager.Instance.MarkerParent);

            var parentScale = transform.parent.localScale;

            rectTransform.localScale = new Vector3(1 / parentScale.x, 1 / parentScale.y, 1 / parentScale.z);

            if (photonView.IsMine)
            {
                rectTransform.localPosition = new Vector3(
                    Random.Range(-0.5f, 0.5f),
                    Random.Range(-0.5f, 0.5f),
                    0);
                SaveToProperty();

                LobbyManager.Instance.MarkerParent.GetComponent<Button>().onClick.AddListener(SetTarget);
            }
        }

        // Update is called once per frame
        void Update () {
		
        }

        public void SetTarget()
        {
            transform.position = Input.mousePosition;
            SaveToProperty();

        }

        private void SaveToProperty()
        {
            PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable
            {
                {"Pos", JsonUtility.ToJson(transform.localPosition)}
            });
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                stream.SendNext(transform.localPosition);
                //stream.SendNext(Text.color);
            }
            else
            {
                // Network player, receive data
                transform.localPosition = (Vector3)stream.ReceiveNext();
                //Text.color = (Color)stream.ReceiveNext();
            }

        }
    }
}
