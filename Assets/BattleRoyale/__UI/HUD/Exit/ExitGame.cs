using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ExitGame : MonoBehaviour
{
    public UnityEvent OnInputEscape;

    public void ExitMach()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            OnInputEscape.Invoke();
        }
    }
}
