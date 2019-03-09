using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ProbSelectButton : MonoBehaviour
{
    public Image Border;
    public RawImage ButtonImage;
    private Button _button;

    public Color notAvailableColor;

    public Color BorderSelectColor;
    public Color borderBaseColor;

    public void SetProbButton(Texture2D image, int active, ProbSelectList list, bool hasProb, bool isCurrent)
    {
        _button = GetComponent<Button>();
        //_button.interactable = hasProb;
        Border.color = isCurrent ? BorderSelectColor : borderBaseColor;
        ButtonImage.color = hasProb ? Color.white : notAvailableColor;

        _button.onClick.RemoveAllListeners();
        if (hasProb)
            _button.onClick.AddListener(() => list.ShowProb(active));
        else
            _button.onClick.AddListener(() => list.PreviewProb(active));
        
        ButtonImage.texture = image;
        
    }
}