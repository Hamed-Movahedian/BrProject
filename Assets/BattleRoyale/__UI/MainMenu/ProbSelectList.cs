using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProbSelectList : MonoBehaviour
{
    public ProbSelectButton ProbButtonPrefab;
    public RectTransform CacheContent;
    public RectTransform Panel;

    [Header("Character")]
    public BrCharacterModel ShowCharacter, MainCharacter;

    [Header("Para")]
    public Para ShowPara;

    [Header("Flag")]
    public Flag ShowFlag;

    public ProfileManager Manager
    {
        get
        {
            if (!_manager)
                _manager = FindObjectOfType<ProfileManager>();
            return _manager;
        }
    }
    public Profile Profile
    {
        get { return _profile ?? (_profile = Manager.PlayerProfile); }
    }

    private Profile _profile;
    private ProfileManager _manager;
    private List<ProbSelectButton> _deactiveButtons = new List<ProbSelectButton>();
    private List<ProbSelectButton> _activeButtons = new List<ProbSelectButton>();
    private ProbType _probsType;
    private int _allProbs;
    private List<int> _availableprobs;
    private int _currentProb,_selectedCharacter,_selectedPara,_selectedFlag,_selectedEmot;
    
    public void CreatPlayerList(int probType)
    {
        GetComponentInChildren<ScrollRect>().verticalNormalizedPosition = 1;
        probType = Mathf.Clamp(probType, 0, 3);
        ShowCharacter.gameObject.SetActive(false);
        ShowFlag.gameObject.SetActive(false);
        ShowPara.gameObject.SetActive(false);

        foreach (ProbSelectButton button in _activeButtons)
        {
            button.gameObject.SetActive(false);
            _deactiveButtons.Add(button);
        }
        _activeButtons.Clear();

        _probsType = (ProbType)probType;

        switch (_probsType)
        {
            case ProbType.Character:
                _availableprobs = Profile.AvalableCharacters;
                _currentProb = _selectedCharacter;
                Manager.CharactersList.Characters[_currentProb].SetToCharacter(ShowCharacter);
                ShowCharacter.gameObject.SetActive(true);
                _allProbs = Manager.CharactersList.Characters.Length;
                break;
            case ProbType.Para:
                _availableprobs = Profile.AvalableParas;
                _currentProb = _selectedPara;
                Manager.ParasList.Paras[_currentProb].SetToPara(ShowPara);
                ShowPara.gameObject.SetActive(true);
                _allProbs = Manager.ParasList.Paras.Length;
                break;
            case ProbType.Flag:
                _availableprobs = Profile.AvalableFlags;
                _currentProb = _selectedFlag;
                Manager.FlagsList.Flags[_currentProb].SetToFlag(ShowFlag);
                ShowFlag.gameObject.SetActive(true);
                _allProbs = Manager.FlagsList.Flags.Length;
                break;
            case ProbType.Emot:
                break;
        }

        for (int probIndex = 0; probIndex < _allProbs; probIndex++)
        {
            ProbSelectButton prefab = GetButton();
            prefab.transform.SetParent(Panel);
            prefab.transform.localScale=Vector3.one;
            prefab.SetProbButton(
                Manager.GetProbIcon(probIndex, _probsType),
                probIndex,
                this,
                _availableprobs.Contains(probIndex)
                );
        }
        SetListSize(_allProbs);
    }

    private void InitializeCurrentProbs(Profile profile)
    {
        _selectedCharacter = profile.CurrentCharacter;
        _selectedEmot = profile.CurrentEmote;
        _selectedFlag = profile.CurrentFlag;
        _selectedPara = profile.CurrentPara;
    }

    private void SetListSize(int buttonsCount)
    {
        VerticalLayoutGroup verticalLayoutGroup = Panel.GetComponent<VerticalLayoutGroup>();
        Vector2 delta = Panel.GetComponent<RectTransform>().sizeDelta;
        delta.y = -verticalLayoutGroup.spacing + verticalLayoutGroup.padding.top + verticalLayoutGroup.padding.bottom;
        delta.y += buttonsCount * (verticalLayoutGroup.spacing + ProbButtonPrefab.GetComponent<RectTransform>().sizeDelta.y);
        Panel.GetComponent<RectTransform>().sizeDelta = delta;
    }
    
    public void ShowProb(int active)
    {
        switch (_probsType)
        {
            case ProbType.Character:
                Manager.CharactersList.Characters[active].SetToCharacter(ShowCharacter);
                _selectedCharacter = active;
                break;
            case ProbType.Para:
                Manager.ParasList.Paras[active].SetToPara(ShowPara);
                _selectedPara = active;
                break;
            case ProbType.Flag:
                Manager.FlagsList.Flags[active].SetToFlag(ShowFlag);
                ShowFlag.GetComponent<Animator>().SetTrigger("Reset");
                _selectedFlag = active;
                break;
            case ProbType.Emot:
                Manager.CharactersList.Characters[active].SetToCharacter(ShowCharacter);
                _selectedEmot = active;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        //_currentProb = active;
    }

    public void SaveProfile()
    {
        Manager.CharactersList.Characters[_selectedCharacter].SetToCharacter(MainCharacter);
        Profile.CurrentCharacter = _selectedCharacter;
        Profile.CurrentFlag = _selectedFlag;
        Profile.CurrentPara= _selectedPara;
        Profile.CurrentEmote= _selectedEmot;
        Manager.SaveProfile();
    }

    private void OnDisable()
    {
        ClearButtonList();
    }

    private void OnEnable()
    {
        InitializeCurrentProbs(Manager.PlayerProfile);
    }
    
    public void ClearButtonList()
    {
        foreach (ProbSelectButton button in _activeButtons)
        {
            button.transform.SetParent(CacheContent);
            _deactiveButtons.Add(button);
        }
        _activeButtons.Clear();
    }

    private ProbSelectButton GetButton()
    {
        ProbSelectButton newButton;
        if (_deactiveButtons.Count > 0)
        {
            newButton = _deactiveButtons[0];
            _deactiveButtons.RemoveAt(0);
        }
        else
        {
            newButton = Instantiate(ProbButtonPrefab);
        }
        _activeButtons.Add(newButton);
        return newButton;
    }

}

[Serializable]
public enum ProbType
{
    Character, Para, Flag, Emot
}