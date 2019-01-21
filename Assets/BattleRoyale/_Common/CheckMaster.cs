using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace BR
{
    public class CheckMaster : MonoBehaviour
    {
        public Text Text;
        // Update is called once per frame
        void Update ()
        {
            if (PhotonNetwork.IsMasterClient)
                Text.text = "Master";
            else
            {
                Text.text = "";
            }
            Text.text += " " + BrCharacterController.MasterCharacter?.profile?.UserID;
        }
    }
}
