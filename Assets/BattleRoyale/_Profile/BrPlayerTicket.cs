using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrPlayerTicket : MonoBehaviour
{
    private void OnEnable() => GetComponent<Text>().text=
        PersianFixer.Fix(
            ProfileManager.Instance().PlayerProfile.TicketCount.ToString());
}
