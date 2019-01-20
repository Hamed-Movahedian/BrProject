using System;
using UnityEngine;
using UnityEngine.UI;

public class BrActivePlayerStatUI : MonoBehaviour
{
    public GameObject panle;
    public Text text;
    private BrCharacterController player;

    #region Instance

    private static BrActivePlayerStatUI _instance;

    public static BrActivePlayerStatUI Instance => 
        _instance ?? (_instance = FindObjectOfType<BrActivePlayerStatUI>());

    #endregion

    private void Awake()
    {
        BrPlayerTracker.Instance.OnActivePlayerChange += (preActivePlayer, nextActivePlayer) =>
        {
            player = nextActivePlayer;

            if (preActivePlayer && !preActivePlayer.isMine)
                Show();
        };
    }

    internal void Show()
    {
        panle.gameObject.SetActive(true);
        text.gameObject.SetActive(true);

        if(player)
            text.text = player.UserID;
    }
}