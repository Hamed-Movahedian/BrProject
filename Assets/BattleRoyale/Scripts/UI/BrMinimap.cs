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
        if (BrPlayerTracker.instance.activePlayer != null)
            MapImage.localPosition = coordinator.NormalizePos(
                BrPlayerTracker.instance.activePlayer.transform.position) *-200;
    }
}
