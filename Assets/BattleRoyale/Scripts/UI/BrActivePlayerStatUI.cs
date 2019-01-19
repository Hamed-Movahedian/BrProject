using System;
using UnityEngine;
using UnityEngine.UI;

public class BrActivePlayerStatUI : MonoBehaviour
{
    public GameObject panle;
    public Text text;
    public static BrActivePlayerStatUI instance;
    private BrCharacterController player;

    private void Awake()
    {
        instance = this;
        BrPlayerTracker.Instance.OnActivePlayerChange += OnActivePlayerChange;
    }

    private void OnActivePlayerChange(BrCharacterController preActivePlayer, BrCharacterController nextActivePlayer)
    {
        player = nextActivePlayer;
        if (preActivePlayer && !preActivePlayer.isMine)
            Show();
    }

    internal void Show()
    {
        panle.gameObject.SetActive(true);
        text.gameObject.SetActive(true);

        if(player)
            text.text = player.UserID;
    }
}