using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class BrAiToggle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Toggle toggle = FindObjectOfType<Toggle>();

        toggle.onValueChanged.AddListener(ChangeAi);
        toggle.onValueChanged.Invoke(toggle.isOn);
    }

    public void ChangeAi(bool haveAi)
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable
        {
            {"AI", haveAi ? 1 : 0}
        });
        Debug.Log(haveAi);
    }
}
