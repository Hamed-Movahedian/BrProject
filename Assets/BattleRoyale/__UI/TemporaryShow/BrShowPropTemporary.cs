using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class BrShowPropTemporary : MonoBehaviour
{
    [Header("Character")] public GameObject Character;
    public CharactersList CharactersList;
    public GameObject CharacterImage;
    [Header("Para")] public GameObject Para;
    public ParasList ParasList;
    public GameObject ParaImage;
    [Header("Flag")] public GameObject Flag;
    public FlagsList FlagsList;
    public GameObject FlagImage;

    [Header("References")] public BrRewardProgress brRewardProgress;
    public ProbSelectList probSelectList;

    [Header("Window Att")] public Text Title;
    public Text Description;
    public Button BuyButton;
    [Multiline] public string PurchaseItemDescribe;
    private ProfileManager profileManager;


    public void ShowProb(ProbType type, int index, bool needBP = false)
    {
        Character.SetActive(false);
        Flag.SetActive(false);
        Para.SetActive(false);
        CharacterImage.SetActive(false);
        ParaImage.SetActive(false);
        FlagImage.SetActive(false);
        BuyButton.gameObject.SetActive(false);
        Description.gameObject.SetActive(false);
        switch (type)
        {
            case ProbType.Character:
                CharactersList[index].SetToCharacter(Character.GetComponent<BrCharacterModel>());
                Character.SetActive(true);
                CharacterImage.SetActive(true);

                break;
            case ProbType.Para:
                ParasList[index].SetToPara(Para.GetComponent<Para>());
                Para.SetActive(true);
                ParaImage.SetActive(true);

                break;
            case ProbType.Flag:
                FlagsList[index].SetToFlag(Flag.GetComponent<Flag>());
                Flag.SetActive(true);
                FlagImage.SetActive(true);

                break;
            case ProbType.Emot:
                CharactersList[index].SetToCharacter(Character.GetComponent<BrCharacterModel>());
                Character.SetActive(true);
                CharacterImage.SetActive(true);

                break;
            default:
                break;
        }

        GetComponent<PlayableDirector>().Play();
    }

    // Start is called before the first frame update
    void Start()
    {
        profileManager = ProfileManager.Instance();

        probSelectList.OnLockProbSelected += PreviewProb;
        BrStoreList.Instance.OnProbSelected += ShowBuyItem;
        //brRewardProgress.OnProbSelected += ShowProb;
        brRewardProgress.OnProbSelectednew += PreviewProb;
    }


    public void PreviewProb(ProbType type, int index, string title, string describe, bool needBP = false)
    {
        Title.text = title;
        Description.text = describe;

        Character.SetActive(false);
        Flag.SetActive(false);
        Para.SetActive(false);
        CharacterImage.SetActive(false);
        ParaImage.SetActive(false);
        FlagImage.SetActive(false);
        BuyButton.gameObject.SetActive(false);
        //Description.gameObject.SetActive(false);

        switch (type)
        {
            case ProbType.Character:
                CharactersList[index].SetToCharacter(Character.GetComponent<BrCharacterModel>());
                Character.SetActive(true);
                CharacterImage.SetActive(true);

                break;
            case ProbType.Para:
                ParasList[index].SetToPara(Para.GetComponent<Para>());
                Para.SetActive(true);
                ParaImage.SetActive(true);

                break;
            case ProbType.Flag:
                FlagsList[index].SetToFlag(Flag.GetComponent<Flag>());
                Flag.SetActive(true);
                FlagImage.SetActive(true);

                break;
            case ProbType.Emot:
                CharactersList[index].SetToCharacter(Character.GetComponent<BrCharacterModel>());
                Character.SetActive(true);
                CharacterImage.SetActive(true);

                break;
            default:
                break;
        }

        GetComponent<PlayableDirector>().Play();
    }

    private void ShowBuyItem(ProbType type, int index, int price)
    {
        string describe = PurchaseItemDescribe.Replace("****", PersianFixer.Fix(price.ToString()));
        string title = PersianFixer.Fix("خرید");
        PreviewProb(type, index, title,describe);
        BuyButton.gameObject.SetActive(true);
        BuyButton.onClick.RemoveAllListeners();
        BuyButton.onClick.AddListener(() => BrStoreList.Instance.BuyItem(type, index, price));
        Description.gameObject.SetActive(true);
    }
}