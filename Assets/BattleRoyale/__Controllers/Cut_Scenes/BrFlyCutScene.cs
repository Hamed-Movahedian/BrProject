using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class BrFlyCutScene : MonoBehaviour
{
    #region Instance

    private static BrFlyCutScene _instance;

    public static BrFlyCutScene Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<BrFlyCutScene>();
            return _instance;
        }
    }

    #endregion

    public PlayableDirector Director;
    
    [Header("Events")]
    public UnityEvent OnStartFalling;
    public UnityEvent OnOpenPara;
    public UnityEvent OnLanding;
    

    // ********************************** methods
    
    public void StartCutScene()
    {
        Director.Play();
    }
    
    public void OpenPara()
    {
        OnOpenPara.Invoke();
    }

    public void Land()
    {
        OnLanding.Invoke();
    }
}
