using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

[CreateAssetMenu(fileName = "ServerController", menuName = "BattleRoyal/Server Controller")]
public class BrServerController : ScriptableObject
{
    #region Instance

    private static BrServerController instance;

    public static BrServerController Instance
    {
        get
        {
            if (instance == null)
                instance = Resources.Load<BrServerController>("ServerController");
            return instance;
        }
    }

    #endregion

    public string URL = @"http://localhost:3794";

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

    public IEnumerator Post(string url, string bodyData,
        Action<string> onSuccess,
        Action<UnityWebRequest> onError = null)
    {
        UnityWebRequest request = PostRequest(url, bodyData);

        var asyncOperation = request.SendWebRequest();

        yield return asyncOperation;

        if (!request.isHttpError && !request.isNetworkError)
        {
            onSuccess?.Invoke(request.downloadHandler.text);
        }
        else
        {
            onError?.Invoke(request);
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
                $"Success \n{text}"
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
}