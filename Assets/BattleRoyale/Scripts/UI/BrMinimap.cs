using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class BrMinimap : MonoBehaviour
{
    private BrLevelCoordinator coordinator;

    public RectTransform MapImage;
	// Use this for initialization
	void Start ()
	{
	    coordinator = FindObjectOfType<BrLevelCoordinator>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (BrPlayerTracker.Instance.ActivePlayer != null)
            MapImage.localPosition = coordinator.NormalizePos(
                BrPlayerTracker.Instance.ActivePlayer.transform.position) *-200;
    }
}
