using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class BrWinLose : MonoBehaviour
{
    public BrCharacterModel CharacterModel;
    
    [Header("Local winner")]
    public Text LocalWinerName;
    public PlayableDirector WinnerDirector;
    
    [Header("RemoteWinner")]
    public Text RemoteWinerName;
    public Text StatText;
    public Image Health;
    public Image Shield;
    public PlayableDirector RemoteWinerDirector;

    private void Awake()
    {
        BrPlayerTracker.Instance.OnLastPlayerLeft += player =>
        {
            CharacterModel.SetProfile(player.profile);

            if (player.isMine)
                Win(player);
            else
                Lose(player);

        };
    }

    private void Lose(BrCharacterController player)
    {
        var profile = player.profile;
        
        RemoteWinerName.text = player.UserID;
        
        StatText.text =
            BrExpManager.CalLevel(profile.PlayerStat.Experience) + "\n" +
            profile.PlayerStat.TotalWins+ "\n" +
            profile.PlayerStat.TotalKills;

        Health.fillAmount = player.Health / (float) player.MaxHealth;
        Shield.fillAmount = player.Shield / (float) player.MaxShield;
        
        RemoteWinerDirector.Play();
    }

    private void Win(BrCharacterController player)
    {
        LocalWinerName.text = player.UserID;
        WinnerDirector.Play();
    }
}
