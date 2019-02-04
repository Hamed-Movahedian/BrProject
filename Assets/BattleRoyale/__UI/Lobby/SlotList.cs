using System;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace BR.Lobby
{
    public class SlotList : MonoBehaviourPunCallbacks
    {
        private const int PlayerCount = 20;
        public List<BrPlayerSlot> SlotImages;
        public CharactersList CharactersList;

        private readonly Player[] _players=new Player[PlayerCount];

        private Color _bgColor;
        
        public static SlotList Instance;

        // Use this for initialization
        void Start ()
        {
            Instance = this;
            _bgColor = SlotImages[0].Border.color;
            
            for (int i = 0; i < PlayerCount; i++)
            {
                _players[i] = null;
            }

            foreach (var player in PhotonNetwork.PlayerList)
            {
                Add(player, true);
            }
        }


        private void Add(Player player, bool silent)
        {
            for (int i = 0; i < PlayerCount; i++)
            {
                if (_players[i] == null)
                {
                    _players[i] = player;

                    
                    SlotImages[i].SetSlot(player,silent);
                    return;
                }
            }
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Add(newPlayer, false);
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
                    SlotImages[i].Remove();
                    _players[i] = null;
                    return;
                }
            }
        }

    }
}
