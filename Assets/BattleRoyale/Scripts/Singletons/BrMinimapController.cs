using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrMinimapController : MonoBehaviour
{
    public Camera MiniCamera;

    public float expandDuration = .2f;

    public RectTransform minimapImage;
    private bool isExpanded = false;

    private float _camY;
    private Vector3 startCenter;
    private float startSize;

    // Use this for initialization
	void Start ()
	{
	    _camY = MiniCamera.transform.localEulerAngles.y;
	}

    public void ToggleExpand()
    {
        if (isExpanded)
            StartCoroutine(Collapse());
        else
        {
            StartCoroutine(Expand());
        }
    }

    private IEnumerator Expand()
    {
        isExpanded = true;
        var targetCenter = BrLevelCoordinator.instance.Center;
        targetCenter.y = MiniCamera.transform.position.y;

        startCenter = MiniCamera.transform.position;
        startSize = MiniCamera.orthographicSize;

        while (minimapImage.localPosition.z <1)
        {
            setMinicamValues(targetCenter, minimapImage.localPosition.z);
            yield return null;
        }
        setMinicamValues(targetCenter, 1);

    }

    private void setMinicamValues(Vector3 targetCenter, float v)
    {
        MiniCamera.transform.position = Vector3.Lerp(
            startCenter,
            targetCenter,
            v);

        MiniCamera.orthographicSize = Mathf.Lerp(
            startSize,
            BrLevelCoordinator.instance.levelBound.size.x / 4,
            v);

        MiniCamera.transform.localEulerAngles = new Vector3(
            MiniCamera.transform.localEulerAngles.x,
            Mathf.LerpAngle(_camY, 0, v),
            MiniCamera.transform.localEulerAngles.z
        );
    }

    private IEnumerator Collapse()
    {
        var targetCenter = BrLevelCoordinator.instance.Center;
        targetCenter.y = MiniCamera.transform.position.y;

        while (minimapImage.localPosition.z >0)
        {
            setMinicamValues(targetCenter, minimapImage.localPosition.z);
            yield return null;
        }
        setMinicamValues(targetCenter, 0);

        isExpanded = false;

    }

    // Update is called once per frame
    void Update ()
	{
	    if (isExpanded)
	        return;

	    if (BrPlayerTracker.Instance.ActivePlayer)
	    {
	        var position = BrPlayerTracker.Instance.ActivePlayer.transform.position;
	        position.y = MiniCamera.transform.position.y;
	        MiniCamera.transform.position = position;

	    }

	}
}
