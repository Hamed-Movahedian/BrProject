using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BR
{
    //[RequireComponent(typeof(InputField))]
    public class PlayerName : MonoBehaviour
    {
        #region Private Constants

        const char IdSeparator = '#';

        #endregion

        public UnityEvent OnNoNameSubmited;
        public Text NameText;
        public Text UserId;


        #region MonoBehaviour CallBacks

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during initialization phase.
        /// </summary>
        void OnEnable()
        {
            string name = ProfileManager.Instance().PlayerProfile.Name;
            Debug.Log(name);
            Debug.Log(name.Length);
            if (name.Replace(" ","").Length<2)
            {
                OnNoNameSubmited.Invoke();
                return;
            }

            NameText.text = name;
            UserId.text = ProfileManager.Instance().PlayerProfile.UserID;

            PhotonNetwork.NickName = name;
        }

        #endregion


        #region Public Methods

        /// <summary>
        /// Sets the name of the player, and save it in the PlayerPrefs for future sessions.
        /// </summary>
        /// <param name="value">The name of the Player</param>
        public void SetPlayerName(string value)
        {
            // #Important
            if (string.IsNullOrEmpty(value))
            {
                Debug.LogError("Player Name is null or empty");
                return;
            }

            ProfileManager.Instance().PlayerProfile.Name = value;
            PhotonNetwork.NickName = value;
        }

        #endregion
    }
}