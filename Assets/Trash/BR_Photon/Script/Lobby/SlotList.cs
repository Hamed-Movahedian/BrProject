using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace BR.Lobby
{
    public class SlotList : MonoBehaviourPunCallbacks
    {
        private const int PlayerCount = 3;
        public List<Image> SlotImages;

        private readonly Player[] _players=new Player[PlayerCount];

        private Color _bgColor;

        // Use this for initialization
        void Start ()
        {
            _bgColor = SlotImages[0].color;

            for (int i = 0; i < PlayerCount; i++)
            {
                _players[i] = null;
            }

            foreach (var player in PhotonNetwork.PlayerList)
            {
                Add(player);
            }
        }

        private void Add(Player player)
        {
            for (int i = 0; i < PlayerCount; i++)
            {
                if (_players[i] == null)
                {
                    _players[i] = player;
                    
                    SlotImages[i].color= JsonUtility.FromJson<Color>((string)player.CustomProperties["Color"]);
                    return;
                }
            }
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Add(newPlayer);
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Remove(otherPlayer);
        }

        private void Remove(Player otherPlayer)
        {
            for (int i = 0; i < PlayerCount; i++)
            {
                if (_players[i].CustomProperties["ID"] == otherPlayer.CustomProperties["ID"])
                {
                    SlotImages[i].color = _bgColor;
                    _players[i] = null;
                    return;
                }
            }
        }

    }
}
