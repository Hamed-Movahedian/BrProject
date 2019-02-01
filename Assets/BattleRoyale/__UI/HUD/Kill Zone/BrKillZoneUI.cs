using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BrKillZoneUI : MonoBehaviour
{
    public Text counter;
    public RectTransform Arrow;

    public UnityEvent OnFirstArea;
    public UnityEvent OnNewArea;
    public UnityEvent OnStartShrinking;

    private Vector3 center;
    private float radoious;
    private BrCharacterController masterCharacter;
    private bool firstTime = true;
    private bool isShirinking = false;

    private void Awake()
    {
        // Get local player
        BrPlayerTracker.Instance.OnMasterPlayerRegister += player => masterCharacter = player;

        // New circle
        BrKillZone.Instance.OnNewCircle += (center, radoious) =>
        {
            this.center = center;
            this.radoious = radoious;

            if (firstTime)
                OnFirstArea.Invoke();
            else
                OnNewArea.Invoke();

            firstTime = false;
            isShirinking = false;
        };

        // Shrinking
        BrKillZone.Instance.Shrinking += time =>
        {
            if (!isShirinking)
            {
                OnStartShrinking.Invoke();
                isShirinking = true;
            }

            counter.text = time.ToString() + "s";
        };

        // Wait for shrink
        BrKillZone.Instance.OnWaitForShrink += time =>
        {
            counter.text = time.ToString() + "s";
        };
    }

    private void Update()
    {
        if (masterCharacter == null)
            return;

        var dir = center - masterCharacter.transform.position;
        dir.y = 0;
        if (dir.sqrMagnitude > radoious * radoious)
        {
            Arrow.gameObject.SetActive(true);
            
            Arrow.eulerAngles = new Vector3(0, 0,
                BrCamera.Instance.MainCamera.transform.eulerAngles.y - Quaternion.LookRotation(dir).eulerAngles.y
            );
        }
        else
            Arrow.gameObject.SetActive(false);
    }
}