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

    public int playerCount = 3;
    
    // Start is called before the first frame update
    IEnumerator Start()
    {
        DontDestroyOnLoad(gameObject);
        
        while (SceneManager.GetActiveScene().name != "MainMenu")
            yield return null;

        yield return new WaitForSeconds(0.1f);

        Toggle toggle = FindObjectOfType<Toggle>();

        toggle.isOn = HaveAI;
        
        while (SceneManager.GetActiveScene().name != "Lobby")
            yield return null;

        yield return  new WaitForSeconds(0.1f);

        LobbyManager.Instance.ArenaLoading = false;
        
        print("Wait for players connect");

        // Wait for players to connect
        yield return 
            new WaitWhile(()=>PhotonNetwork.CurrentRoom.PlayerCount<playerCount);

        yield return  new WaitForSeconds(1);

        print("players connected");

        if (PhotonNetwork.IsMasterClient)
        {
            print("Arena load");
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.LoadLevel("Arena");
        }
        
        print("Wait for arena");

        while (SceneManager.GetActiveScene().name != "Arena")
            yield return null;

        //yield return  new WaitForSeconds(0.1f);
        
        BrFlyCutScene.Instance.OnStartFalling.AddListener(() =>
        {
            var director = FindObjectOfType<BrFlyCutScene>().GetComponent<PlayableDirector>();

 
            director.Pause();
            director.time = 226 / 60d;
            director.Evaluate();
            director.Play();
        });

/*
        FindObjectOfType<LobbyManager>().CloseRoom();

        FindObjectOfType<LobbyManager>().LoadArena();
        
        print("Wait for arena to load");

        while (SceneManager.GetActiveScene().name != "Arena")
            yield return null;


        print("Arena loaded");

        yield return  new WaitForSeconds(1);
*/
        
        
    }


}