using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrKillZoneUI : MonoBehaviour
{
    public Image ring;
    public Image arrowImage;
    public Text counter;
    public GameObject NewArea;
    public RectTransform Arrow;


    private Vector3 center;
    private float radoious;

    private void Awake()
    {
        // New circle
        BrKillZone.Instance.OnNewCircle += (center, radoious) =>
        {
            this.center = center;
            this.radoious = radoious;
            NewArea.SetActive(true);
            Invoke("DisableNewAreaGameObject", 1);
        };

        // Shrinking
        BrKillZone.Instance.Shrinking += time =>
        {
            if (time == 0)
            {
                ring.gameObject.SetActive(false);
                return;
            }
            counter.text = time.ToString() + "s";

            ring.color =
                counter.color =
                    arrowImage.color = Color.red;
        };

        // Wait for shrink
        BrKillZone.Instance.OnWaitForShrink += time =>
        {
            counter.text = time.ToString() + "s";
        };

        ring.gameObject.SetActive(false);

    }
    
    private void DisableNewAreaGameObject()
    {
        NewArea.SetActive(false);
        ring.gameObject.SetActive(true);
        ring.color =
            counter.color =
            arrowImage.color = Color.white;
    }

    private void Update()
    {
        BrCharacterController masterCharacter = BrCharacterController.MasterCharacter;
        if (masterCharacter == null)
            return;

        var dir = center - masterCharacter.transform.position;
        dir.y = 0;
        if (dir.sqrMagnitude > radoious * radoious)
        {
            Arrow.gameObject.SetActive(true);

            var rot = Quaternion.LookRotation(dir).eulerAngles;
            Arrow.eulerAngles = new Vector3(0, 0,
                BrCamera.Instance.MainCamera.transform.eulerAngles.y - Quaternion.LookRotation(dir).eulerAngles.y
                );
        }
        else
            Arrow.gameObject.SetActive(false);
    }
}
