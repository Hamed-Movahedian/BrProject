using System;
using System.Collections;
using System.Collections.Generic;
using TapsellSDK;
using UnityEngine;
using UnityEngine.UI;

public class AdVideoManager : MonoBehaviour
{
    private string _zoneId = "5c1408f6a4973c0001c5efaa";
    private bool _cached = false;
    private TapsellAd _ad;

    private bool _isReady;

    public bool VideoIsReady
    {
        get { return _isReady; }
        set
        {
            _isReady = value;
            GetComponent<Button>().interactable =_isReady ;
        } 
    }

    private void OnEnable()
    {
        GetAdVideo();
    }


    void Start()
    {

    }


    public void GetAdVideo()
    {
        VideoIsReady = false;
        Tapsell.requestAd(_zoneId, _cached,
            (TapsellAd result) =>
            {
                // onAdAvailable
                Debug.Log("Action: onAdAvailable");
                _ad = result; // store this to show the ad later
                VideoIsReady = true;
            },

            (string zoneId) =>
            {
                // onNoAdAvailable
                Debug.Log("No Ad Available");
                Invoke("GetAdVideo", 30);
            },

            (TapsellError error) =>
            {
                // onError
                Debug.Log(error.error);
                Invoke("GetAdVideo", 30);
            },

            (string zoneId) =>
            {
                // onNoNetwork
                Debug.Log("No Network");
                Invoke("GetAdVideo",30);
            },

            (TapsellAd result) =>
            {
                // onExpiring
                Debug.Log("Expiring");
                GetAdVideo();
            }
        );
    }


    public void ShowAdVideo()
    {
        TapsellShowOptions showOptions = new TapsellShowOptions();
        showOptions.backDisabled = true;
        showOptions.immersiveMode = false;
        showOptions.rotationMode = TapsellShowOptions.ROTATION_UNLOCKED;
        showOptions.showDialog = true;

        Tapsell.showAd(_ad, showOptions);
    }


}
