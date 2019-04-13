using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BrSetPlayerName : MonoBehaviour
{

    public InputField NameInput;
    public Button Submit, Cancel;
    public Color32 Available, Forbiden;

    public UnityEvent OnNameSubmited;
    
    public void GetPlayerName()
    {
        string userID = ProfileManager.Instance().PlayerProfile.UserID;

        if (userID == "")
        {
            Cancel.interactable = false;
            return;
        }

        var nickName = userID.Split('#')[0];
        NameInput.text = nickName;
        Cancel.interactable = true;
    }

    public void SetPlayerName()
    {
        string value = NameInput.text;

        // #Important
        if (string.IsNullOrEmpty(value))
        {
            Debug.LogError("Player Name is null or empty");
            return;
        }

        ProfileManager.Instance().PlayerProfile.UserID = value+"#"+GetPlayerCode();
        ProfileManager.Instance().SaveProfile();
        PhotonNetwork.NickName = value;
        OnNameSubmited.Invoke();
    }

    private string GetPlayerCode()
    {
        return SystemInfo.deviceUniqueIdentifier.Substring(0, 4);
    }

    public void CheckNameAvailability()
    {
        bool b = NameInput.text.Length > 5;
        NameInput.textComponent.color = b ? Available : Forbiden;
        Submit.interactable= b;
    }

}
