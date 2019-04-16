using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrBattlePassButton : MonoBehaviour
{
    public int Price;
    public Text ButtonText;
    public string BuyText,HaveText;


    private Button _button;
    private bool _hasBP;

    private void OnEnable()
    {
        _button = GetComponent<Button>();
        _hasBP = ProfileManager.Instance().PlayerProfile.HasBattlePass;

        
        Initialize();
    }

    private void Initialize()
    {
        _button.onClick.RemoveAllListeners();
        if (_hasBP)
        {
            _button.interactable=false;
            ButtonText.text = HaveText;
            return;
        }

        _button.interactable = true;
        ButtonText.text = BuyText;
        _button.onClick.AddListener((() => BrStoreList.Instance.BuyBattlePass(Price)));
    }

}
