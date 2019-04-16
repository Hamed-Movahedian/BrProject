using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrSeverUI : MonoBehaviour
{
    public GameObject ButtonPanel;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        gameObject.SetActive(false);
        ButtonPanel.SetActive(false);
        
        BrServerController.Instance.OnSend += request =>
        {
            gameObject.SetActive(true);
        };

        BrServerController.Instance.OnSuccess += request =>
        {
            gameObject.SetActive(false);
        };

        BrServerController.Instance.OnFail += request =>
        {
            ButtonPanel.SetActive(true);
        };
    }

    public void Retry()
    {
        BrServerController.Instance.Retry();
        ButtonPanel.SetActive(false);

    }

    public void Exit()
    {
        Application.Quit();
        ButtonPanel.SetActive(false);
    }
}
