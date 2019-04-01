using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BrStoreList : MonoBehaviour
{


    public BrStoreItemButton ButtonPrefab;

    public RectTransform InGameItemsContent;
    public RectTransform TicketsContent;
    public RectTransform BazaarContent;
    public RectTransform CacheContent;

    public List<InGameStoreItem> StoreItems;
    public List<TicketPack> TicketPacks;
    public List<MarketItem> PurchaseItems;

    public CharactersList CharactersList;
    public ParasList ParasList;
    public FlagsList FlagsList;

    public delegate void SelectLockProb(ProbType type, int index, int price);

    [HideInInspector] public SelectLockProb OnProbSelected;

    public UnityEvent OnLowCoin;

    private List<BrStoreItemButton> _activeButtons = new List<BrStoreItemButton>();
    private List<BrStoreItemButton> _deactiveButtons = new List<BrStoreItemButton>();
    private bool marketIsReady;


    #region Instance

    private static BrStoreList instance;
    private ProfileManager _profileManage;

    public static BrStoreList Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<BrStoreList>();
            return instance;
        }
    }

    #endregion


    void Start()
    {
        _profileManage = ProfileManager.Instance();

        PurchaseManager.Instance.OnItemPurchased += PurchaseSucceed;

        ClearButtonList();
        
        foreach (InGameStoreItem item in StoreItems)
        {
            if (ProfileManager.Instance().HaveItem(item.ItemType, item.Index))
                continue;
            Sprite icon = null;
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
            button.GetComponent<RectTransform>().SetParent(InGameItemsContent);
            button.transform.localScale = Vector3.one;
            button.InitializeButton(
                icon,
                item.Price,
                0,
                MarketItemType.Prob,
                delegate { Selected(item.ItemType, item.Index, item.Price); });
        }
        
        foreach (TicketPack item in TicketPacks)
        {
            BrStoreItemButton button = GetButton();
            button.GetComponent<RectTransform>().SetParent(TicketsContent);
            button.transform.localScale = Vector3.one;
            button.InitializeButton(
                item.ItemIcon,
                item.Price,
                item.TicketCount,
                MarketItemType.Ticket,
                delegate { AddTicket(item); });
        }

        foreach (MarketItem item in PurchaseItems)
        {
            BrStoreItemButton button = GetButton();
            button.GetComponent<RectTransform>().SetParent(BazaarContent);
            button.transform.localScale = Vector3.one;
            button.InitializeButton(
                item.ItemIcon,
                item.Price,
                item.CoinCount,
                MarketItemType.Coin,
                delegate { Purchase(item); });
        }
    }

    private void AddTicket(TicketPack ticketPack)
    {
        
    }

    private void PurchaseSucceed(string itemId)
    {
        Debug.Log(itemId);
        foreach (MarketItem item in PurchaseItems)
        {
            if (item.ItemId == itemId)
            {
                ProfileManager.Instance().PlayerProfile.CoinCount += item.CoinCount;
                ProfileManager.Instance().SaveProfile();
                PurchaseManager.Instance.OnCoinCountChanged?.Invoke(ProfileManager.Instance().PlayerProfile.CoinCount);
                PurchaseManager.Consume(itemId);
                return;
            }
        }
    }

    private void Purchase(MarketItem item)
    {
        Debug.Log("Purchase started in app");
        PurchaseManager.Instance.BuyItem(item.ItemId);
    }

    private void Selected(ProbType probType, int index, int price)
    {
        OnProbSelected?.Invoke(probType, index, price);
    }

    public void BuyItem(ProbType type, int index, int price)
    {
        if (price > _profileManage.PlayerProfile.CoinCount)
        {
            NeedMoreCoins();
            return;
        }

        _profileManage.PlayerProfile.CoinCount -= price;
        PurchaseManager.Instance.OnCoinCountChanged?.Invoke(_profileManage.PlayerProfile.CoinCount);
        _profileManage.AddProb(type, index);
        ReInitiate();
    }
    
    


    private void NeedMoreCoins()
    {
        print("Need More Coin");
        OnLowCoin.Invoke();
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

    public void ReInitiate()
    {
        Start();
    }

    public void BuyBattlePass(int price)
    {
        if (price > _profileManage.PlayerProfile.CoinCount)
        {
            NeedMoreCoins();
            return;
        }

        _profileManage.PlayerProfile.CoinCount -= price;
        PurchaseManager.Instance.OnCoinCountChanged?.Invoke(_profileManage.PlayerProfile.CoinCount);
        _profileManage.GiveBattlePass();
        ReInitiate();
    }
}

[Serializable]
public struct MarketItem
{
    public int CoinCount;
    public string ItemId;
    public Sprite ItemIcon;
    public int Price;
}

[Serializable]
public struct InGameStoreItem
{
    public ProbType ItemType;
    public int Index;
    public int Price;
}

[Serializable]
public class TicketPack
{
    public int TicketCount;
    public Sprite ItemIcon;
    public int Price;
}