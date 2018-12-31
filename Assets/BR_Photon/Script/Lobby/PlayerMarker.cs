﻿using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace BR.Lobby
{
    public class PlayerMarker : MonoBehaviourPunCallbacks, IPunObservable
    {
        public Text Text;

        // Use this for initialization
        void Start ()
        {
            Text.color = JsonUtility.FromJson<Color>((string) photonView.Owner.CustomProperties["Color"]);
            RectTransform rectTransform = transform as RectTransform;

            rectTransform.SetParent(LobbyManager.Instance.MarkerParent);

            if (photonView.IsMine)
            {
                rectTransform.localPosition = new Vector3(
                    Random.Range(-0.5f, 0.5f),
                    Random.Range(-0.5f, 0.5f),
                    0);

                LobbyManager.Instance.MarkerParent.GetComponent<Button>().onClick.AddListener(SetTarget);
            }
        }

        // Update is called once per frame
        void Update () {
		
        }

        public void SetTarget()
        {
            transform.position = Input.mousePosition;

            PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable
            {
                {"Pos", JsonUtility.ToJson(transform.localPosition)}
            });

            print(transform.localPosition);
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