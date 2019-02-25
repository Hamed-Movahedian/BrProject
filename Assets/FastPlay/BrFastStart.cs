using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BR;
using BR.Lobby;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class BrFastStart : MonoBehaviour
{
    public bool HaveAI = true;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        DontDestroyOnLoad(gameObject);
        while (SceneManager.GetActiveScene().name != "MainMenu")
            yield return null;

        yield return new WaitForSeconds(0.1f);

        Toggle toggle = FindObjectOfType<Toggle>();

        toggle.isOn = HaveAI;

        yield return new WaitForSeconds(0.1f);

        FindObjectOfType<Launcher>().Connect();

        while (SceneManager.GetActiveScene().name != "Lobby")
            yield return null;

        yield return new WaitForSeconds(2);

        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable
        {
            {"Pos", JsonUtility.ToJson(new Vector3(Random.Range(-0.35f, 0.35f), Random.Range(-0.35f, 0.35f), 0))}
        });

        FindObjectOfType<LobbyManager>().CloseRoom();

        FindObjectOfType<LobbyManager>().LoadArena();

        while (SceneManager.GetActiveScene().name != "Arena")
            yield return null;

        var director = FindObjectOfType<BrFlyCutScene>().GetComponent<PlayableDirector>();
        if (director && director.state == PlayState.Playing)
        {
            director.Pause();
            director.time = 226 / 60d;
            director.Evaluate();
            director.Play();
        }
    }


    // Update is called once per frame
    void Update()
    {
    }
}