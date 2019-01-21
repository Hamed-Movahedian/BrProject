using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProbSelectButton : MonoBehaviour
{
    public RawImage ButtonImage;
    private Button _button;
    public void SetProbButton(Texture2D image, int active, ProbSelectList list, bool hasProb)
    {
        _button=GetComponent<Button>();
        _button.interactable = hasProb;
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(() => list.ShowProb(active));
        ButtonImage.texture = image;
    }

}
