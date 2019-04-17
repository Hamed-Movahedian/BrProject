using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BrSeverUI : MonoBehaviour
{
    public GameObject MainPanel;
    public GameObject ButtonPanel;

    public Text Message;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        MainPanel.SetActive(false);
        ButtonPanel.SetActive(false);
        
        BrServerController.Instance.OnSend += request =>
        {
            Message.text = "Connecting...";
            MainPanel.SetActive(true);
        };

        BrServerController.Instance.OnSuccess += request =>
        {
            MainPanel.SetActive(false);
        };

        BrServerController.Instance.OnFail += request =>
        {
            string error = "";

            if (!string.IsNullOrEmpty(request.downloadHandler.text))
            {
                var message = (string)JToken.Parse(request.downloadHandler.text)["Message"];
                if (message != null)
                    error = message;
            }
                
            Message.text = $"{request.error}\n{error}";
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
