using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrMainMenuCharacter : MonoBehaviour
{
    void Start()
    {
        var character = ProfileManager.Instance().PlayerProfile;
        GetComponent<BrCharacterModel>().SetProfile(character);
    }

}
