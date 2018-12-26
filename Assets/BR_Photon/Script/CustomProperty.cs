using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace BR
{
    public class CustomProperty : MonoBehaviour
    {
        public Image Image;
        // Use this for initialization
        public void SetColor()
        {
            Hashtable prop = new Hashtable
            {
                {"Color", JsonUtility.ToJson(Image.color)}
            };

            PhotonNetwork.LocalPlayer.SetCustomProperties(prop);
        }
    }
}
