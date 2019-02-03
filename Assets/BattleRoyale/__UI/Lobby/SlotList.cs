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

        // Use this for initialization
        void Start ()
        {
            //SlotImages = GetComponentsInChildren<Image>().ToList();

            _bgColor = SlotImages[0].Border.color;
            
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

                    var profile = Profile.Deserialize((string)player.CustomProperties["Profile"]);

                    Sprite pIcon= CharactersList[profile.CurrentCharacter].FaceSprite;
                    Color borderColor= JsonUtility.FromJson<Color>((string)player.CustomProperties["Color"]);
                    SlotImages[i].SetSlot(pIcon,borderColor);
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
                    SlotImages[i].Remove();
                    _players[i] = null;
                    return;
                }
            }
        }

    }
}
