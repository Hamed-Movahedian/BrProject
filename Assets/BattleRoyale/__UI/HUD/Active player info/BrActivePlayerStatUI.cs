using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.UI;

public class BrActivePlayerStatUI : MonoBehaviour
{
    public BrCharacterModel CharacterModel;
    public PlayableDirector Director;

    public Text StatText;
    public Text NameText;
    public Image Health;
    public Image Shield;

    public UnityEvent OnHide;
    private BrCharacterController player;

    private bool fristTime = true;
    #region Instance

    private static BrActivePlayerStatUI _instance;
    private Profile profile=null;

    public static BrActivePlayerStatUI Instance => 
        _instance ?? (_instance = FindObjectOfType<BrActivePlayerStatUI>());

    #endregion

    private void Awake()
    {
        BrPlayerTracker.Instance.OnActivePlayerChange += (preActivePlayer, nextActivePlayer) =>
        {
            player = nextActivePlayer;

            if (preActivePlayer && !preActivePlayer.IsMaster)
                Show();
        };

        BrPlayerTracker.Instance.OnLastPlayerLeft += controller => OnHide.Invoke();
    }

    internal void Show()
    {
        profile = player.profile;
        CharacterModel.SetProfile(profile);
        NameText.text = profile.UserID;
        
        UpdateStat();

        if(fristTime)
           Director.Play();
        
        fristTime = false;
    }

    private void UpdateStat()
    {
        StatText.text =
            BrExpManager.CalLevel(profile.PlayerStat.Experience).ToString() + "\n" +
            profile.PlayerStat.TotalWins + "\n" +
            profile.PlayerStat.TotalKills;

        Health.fillAmount = player.Health / (float) player.MaxHealth;
        Shield.fillAmount = player.Shield / (float) player.MaxShield;
    }

    private void FixedUpdate()
    {
        if(profile!=null)
            UpdateStat();
    }
}