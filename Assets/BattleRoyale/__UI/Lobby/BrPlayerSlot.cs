using System.Collections;
using System.Collections.Generic;
using BR.Lobby;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class BrPlayerSlot : MonoBehaviour
{
    public Image Border;
    public Image PlayerIcon;
    private PlayableDirector playableDirector;

    private void OnEnable()
    {
        playableDirector = GetComponent<PlayableDirector>();
    }

    
    public void Remove()
    {
        playableDirector.Play();
    }

    public void SetSlot(Player player, bool silent)
    {
        var profile = Profile.Deserialize((string) player.CustomProperties["Profile"]);

        PlayerIcon.sprite = SlotList.Instance.CharactersList[profile.CurrentCharacter].FaceSprite;
        Border.color = JsonUtility.FromJson<Color>((string) player.CustomProperties["Color"]);

        print($"Silent ={silent}");
        GetComponent<BrTimelineEvent>().SetCondition(silent ? 1 : 0);
        
        playableDirector.Play();
    }
}