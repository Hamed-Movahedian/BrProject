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
        private const int PlayerCount = 10;
        public List<Image> SlotImages;
        public CharactersList CharactersList;

        private readonly Player[] _players=new Player[PlayerCount];

        private Color _bgColor;

        // Use this for initialization
        void Start ()
        {
            //SlotImages = GetComponentsInChildren<Image>().ToList();

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

                    var profile = Profile.Deserialize((string)player.CustomProperties["Profile"]);

                    SlotImages[i].sprite = CharactersList[profile.CurrentCharacter].FaceSprite;
                    SlotImages[i].color = Color.white;
                    SlotImages[i].transform.GetChild(0).GetComponent<Image>().color= 
                        JsonUtility.FromJson<Color>((string)player.CustomProperties["Color"]);
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
