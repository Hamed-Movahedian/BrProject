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
    public BrRewardProgress brRewardProgress;
    public ProbSelectList probSelectList;

    public Text Description;
    public Button BuyButton;
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

        probSelectList.OnLockProbSelected += (type, index) => ShowProb(type, index, false);
        BrStoreList.Instance.OnProbSelected += ShowBuyItem;
        brRewardProgress.OnProbSelected += ShowProb;
    }

    private void ShowBuyItem(ProbType type, int index, int price)
    {
        ShowProb(type, index);
        BuyButton.gameObject.SetActive(true);
        BuyButton.onClick.RemoveAllListeners();
        BuyButton.onClick.AddListener(() => BrStoreList.Instance.BuyItem(type, index, price));
        Description.text = price.ToString();
    }

}