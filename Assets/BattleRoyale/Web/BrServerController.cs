using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class BrServerController : MonoBehaviour
{
    #region Instance

    private static BrServerController instance;

    public static BrServerController Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<BrServerController>();
            return instance;
        }
    }

#endregion

    #region OnSend
    
        public delegate void OnSendDel(UnityWebRequest request);
    
        public OnSendDel OnSend;
    
        #endregion

    #region OnFail
    
        public delegate void OnFailDel(UnityWebRequest request);
    
        public OnFailDel OnFail;
    
        #endregion
        
    #region OnSuccess
    
        public delegate void OnSuccessDel(UnityWebRequest request);
    
        public OnSuccessDel OnSuccess;
    
        #endregion
        
    public string URL = @"http://localhost:3794";
    private UnityWebRequest request;
    private bool waitForRetry;


    #region PostRequst

    private UnityWebRequest PostRequest(string url, string bodyData)
    {
        var finalUrl = URL + @"/api/" + url;

        UnityWebRequest request = new UnityWebRequest(
            finalUrl,
            "POST",
            new DownloadHandlerBuffer(),
            (bodyData == null)
                ? null
                : new UploadHandlerRaw(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(bodyData))));

        request.SetRequestHeader("Content-Type", "application/json");
        

        return request;
    }

    #endregion

    #region GetRequest

    public static UnityWebRequest GetRequest(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(Instance.URL + @"/api/" + url);

        request.SetRequestHeader("Content-Type", "application/json");

        return request;
    }

    #endregion

    #region Post

    public void Post(
        string url,
        string bodyData,
        Action<string> onSuccess,
        Action<UnityWebRequest> onError = null)
    {
        StartCoroutine(PostCoroutine(url, bodyData, onSuccess, onError));
    }

    private IEnumerator PostCoroutine(
        string url, 
        string bodyData,
        Action<string> onSuccess,
        Action<UnityWebRequest> onError = null)
    {
        request = PostRequest(url, bodyData);

        while (true)
        {
            var asyncOperation = request.SendWebRequest();

            yield return asyncOperation;

            if (!request.isHttpError && !request.isNetworkError)
            {
                OnSuccess?.Invoke(request);
                onSuccess?.Invoke(request.downloadHandler.text);
                break;
            }
            else
            {
                onError?.Invoke(request);
                OnFail?.Invoke(request);
                
                waitForRetry = true;
                while (waitForRetry)
                    yield return null;
            }
        }
    }

#if UNITY_EDITOR

    public void PostEditor(string url, string bodyData,
        Action<string> onSuccess,
        Action<UnityWebRequest> onError = null)
    {
        UnityWebRequest request = PostRequest(url, bodyData);

        request.SendWebRequest();

        while (!request.isDone)
            UnityEditor.EditorUtility.DisplayProgressBar(
                "Post Request", $"Url ={url}\nBodyData={bodyData}",
                request.downloadProgress);

        UnityEditor.EditorUtility.ClearProgressBar();


        if (!request.isHttpError && !request.isNetworkError)
        {
            var text = request.downloadHandler.text;

            onSuccess?.Invoke(text);

            UnityEditor.EditorUtility.DisplayDialog(
                "Post Request",
                $"Success"
                , "Ok");
        }
        else
        {
            onError?.Invoke(request);
            UnityEditor.EditorUtility.DisplayDialog(
                "Post Request",
                $"Failed! \n{request.error}"
                , "Ok");
        }
    }
#endif

    #endregion

    public void Retry()
    {
        waitForRetry = false;
    }
}