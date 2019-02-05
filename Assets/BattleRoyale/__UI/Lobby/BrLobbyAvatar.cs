using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrLobbyAvatar : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int character = FindObjectOfType<ProfileManager>().PlayerProfile.CurrentCharacter;
        BrCharacterModel characterModel = GetComponent<BrCharacterModel>();
        characterModel.CharactersList[character].SetToCharacter(characterModel);
    }
}
