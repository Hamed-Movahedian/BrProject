using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class BrPlayerSlot : MonoBehaviour
{

    public Image Border;
    public Image PlayerIcon;


    public void SetSlot(Sprite pIcon, Color pColor)
    {
        //Border.color = pColor;
        PlayerIcon.sprite = pIcon;
        GetComponent<PlayableDirector>().Play();
    }

    public void Remove()
    {
        GetComponent<PlayableDirector>().Play();
    }
}
