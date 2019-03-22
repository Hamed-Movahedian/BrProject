using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BrStoreList : MonoBehaviour
{
    public BrStoreItemButton ButtonPrefab;
    
    public RectTransform Content;
    public RectTransform CacheContent;

    public List<IngameStoreItem> StoreItems;
    public List<MarketItem> PurchaseItems;

    public CharactersList CharactersList;
    public ParasList ParasList;
    public FlagsList FlagsList;
    
    public delegate UnityEvent SelectLockProb(ProbType type, int index);
    [HideInInspector]public SelectLockProb OnProbSelected;
    private List<BrStoreItemButton> _activeButtons=new List<BrStoreItemButton>();
    private List<BrStoreItemButton> _deactiveButtons=new List<BrStoreItemButton>();
    private bool marketIsReady;

    void Start()
    {
        ClearButtonList();
        foreach (IngameStoreItem item in StoreItems)
        {
            Sprite icon=null;
            switch (item.ItemType)
            {
                case ProbType.Character:
                    icon = CharactersList[item.Index].BodySprite;
                    break;
                case ProbType.Para:                    
                    icon = ParasList[item.Index].Sprite;

                    break;
                case ProbType.Flag:                    
                    icon = FlagsList[item.Index].Sprite;

                    break;
                case ProbType.Emot:                    
                    icon = CharactersList[item.Index].BodySprite;

                    break;
                case ProbType.NoProb:
                    break;
            }

            BrStoreItemButton button = GetButton();
            button.GetComponent<RectTransform>().SetParent(Content);
            button.transform.localScale=Vector3.one;
            button.InitializeButton(
                icon,
                item.Price,
                false,
                delegate { Selected(item.ItemType, item.Price); });
        }

        foreach (MarketItem item in PurchaseItems)
        {
            BrStoreItemButton button = GetButton();
            button.GetComponent<RectTransform>().SetParent(Content);
            button.transform.localScale=Vector3.one;
            button.InitializeButton(
                item.ItemIcon,
                item.Price,
                false,
                delegate { Purchase(item.ItemId); });
        }
        
    }

    private void Purchase(string itemId)
    {
        PurchaseManager.BuyItem(itemId);
    }

    private void Selected(ProbType probType, int price)
    {
        OnProbSelected?.Invoke(probType, price);
    }


    public void ClearButtonList()
    {
        foreach (BrStoreItemButton button in _activeButtons)
        {
            button.transform.SetParent(CacheContent);
            _deactiveButtons.Add(button);
        }

        _activeButtons.Clear();
    }

    private BrStoreItemButton GetButton()
    {
        BrStoreItemButton newButton;
        if (_deactiveButtons.Count > 0)
        {
            newButton = _deactiveButtons[0];
            _deactiveButtons.RemoveAt(0);
        }
        else
        {
            newButton = Instantiate(ButtonPrefab);
        }

        _activeButtons.Add(newButton);
        return newButton;
    }

}

[Serializable]
public struct MarketItem
{
    public string ItemId;
    public Sprite ItemIcon;
    public int Price;
}

[Serializable]
public struct IngameStoreItem
{
    public ProbType ItemType;
    public int Index;
    public int Price;
}
