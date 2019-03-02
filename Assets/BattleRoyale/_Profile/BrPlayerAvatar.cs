using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrPlayerAvatar : MonoBehaviour
{
    private void OnEnable()
    {
        int character = ProfileManager.Instance().PlayerProfile.CurrentCharacter;
        GetComponent<RawImage>().texture=ProfileManager.Instance().CharactersList[character].FaceIcon;
    }

}
