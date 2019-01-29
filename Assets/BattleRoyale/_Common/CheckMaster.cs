using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace BR
{
    public class CheckMaster : MonoBehaviour
    {
        public Text Text;
        
        // Update is called once per frame
        void FixedUpdate ()
        {
            Text.text = PhotonNetwork.IsMasterClient ? "Master" : "Normal";
        }
    }
}
