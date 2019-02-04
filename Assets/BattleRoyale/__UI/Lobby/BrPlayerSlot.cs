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

    public void SetSlot(Sprite pIcon, Color pColor)
    {
        //Border.color = pColor;
        PlayerIcon.sprite = pIcon;
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

        GetComponent<BrTimelineEvent>().SetCondition(silent ? 0 : 1);
        playableDirector.Play();
    }
}