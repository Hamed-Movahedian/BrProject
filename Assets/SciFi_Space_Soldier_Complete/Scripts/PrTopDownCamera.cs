using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PrTopDownCamera : MonoBehaviour {

    [Header("Camera Basic Settings")]
    public float FollowSpeed = 1.0f;
    public Transform TargetToFollow;
    //Offset
    //[HideInInspector]
    public float TargetHeight = 0.0f;
    //[HideInInspector]
    public float targetHeightOffset = -1.0f;
    //[HideInInspector]
    public float ActualHeight = 0.0f;
   
    public float HeightSpeed = 3.0f;
    public Transform CameraOffset;

    [Header("Shooting Shake Settings")]
    public bool isShaking = false;
    public float shakeFactor = 3f;
    public float shakeTimer = .2f;
    public float shakeSmoothness = 5f;
    [HideInInspector]
    public float actualShakeTimer = 0.2f;

    [Header("Explosion Shake Settings")]
    public bool isExpShaking = false;
    public float shakeExpFactor = 5f;
    public float shakeExpTimer = 1.0f;
    public float shakeExpSmoothness = 3f;
    [HideInInspector]
    public float actualExpShakeTimer = 1.0f;

    [Header("Movement Shake Settings")]
    public float movShaking = 1.0f;
    private Vector3 randomShakePos = Vector3.zero;

    private bool showBlood = true;
    private float _targetFollowSpeed;
    private PrTopDownCharController _charController;

    [Header("Falling")]
    public float FallingHeight=-3;
    public float FallingShakeFactor = 3f;
    public float FallingShakeSmoothness = 5f;
    public Transform FallingTargetToFollow;

    [Header("Para")]
    public float ParaHeight =-8;
    public float ParaFollowSpeed = 1.0f;

    [Header("Land")]
    public float LandHeight =-6;

    // Use this for initialization
    IEnumerator Start ()
    {
        _charController = transform.parent.GetComponentInChildren<PrTopDownCharController>();
        actualShakeTimer = shakeTimer;
        _targetFollowSpeed = FollowSpeed;

        yield return null;
        //yield return null;
        transform.position = TargetToFollow.position;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.B))
        {
            EnableBlood();
        }
    }

    public Vector3 CalculateRandomShake(float shakeFac, bool isExplosion)
    {
        randomShakePos = new Vector3(Random.Range(-shakeFac, shakeFac), Random.Range(-shakeFac, shakeFac), Random.Range(-shakeFac, shakeFac));
        if (isExplosion)
            return randomShakePos * (actualExpShakeTimer / shakeExpTimer);
        else
            return randomShakePos * (actualShakeTimer / shakeTimer);
    }

    public void Shake(float factor, float duration)
    {
        isShaking = true;
        shakeFactor = factor;
        shakeTimer = duration;
        actualShakeTimer = shakeTimer;
    }

    public void ExplosionShake(float factor, float duration)
    {
        isExpShaking = true;
        shakeExpFactor = factor;
        shakeExpTimer = duration;
        actualExpShakeTimer = shakeExpTimer;
    }

    void LateUpdate()
    {
        if (TargetToFollow)
        {
            if (_charController.Falling && !_charController.IsParaOpen)
            {
                transform.position = FallingTargetToFollow.position;
                ActualHeight = FallingHeight;

                Vector3 newPos = transform.localPosition + CalculateRandomShake(FallingShakeFactor, false);
                transform.localPosition = Vector3.Lerp(transform.localPosition, newPos, FallingShakeSmoothness * Time.deltaTime);

            }
            else if(_charController.Falling && _charController.IsParaOpen)
            {
                TargetHeight = ParaHeight;
                ActualHeight = Mathf.Lerp(ActualHeight, TargetHeight, Time.deltaTime * HeightSpeed);
                transform.position = Vector3.Lerp(transform.position, TargetToFollow.position, ParaFollowSpeed * Time.deltaTime);

            }
            else
            {
                TargetHeight = LandHeight;
                ActualHeight = Mathf.Lerp(ActualHeight, TargetHeight, Time.deltaTime * HeightSpeed);
                transform.position = Vector3.Lerp(transform.position, TargetToFollow.position, FollowSpeed * Time.deltaTime);
            }


            CameraOffset.localPosition = new Vector3(0.0f, 0.0f, ActualHeight);
        }

        if (isShaking && !isExpShaking)
        {
            if (actualShakeTimer >= 0.0f)
            {
                actualShakeTimer -= Time.deltaTime;
                Vector3 newPos = transform.localPosition + CalculateRandomShake(shakeFactor, false);
                transform.localPosition = Vector3.Lerp(transform.localPosition, newPos, shakeSmoothness * Time.deltaTime);
            }
            else
            {
                isShaking = false;
                actualShakeTimer = shakeTimer;
            }
        }

        else if (isExpShaking)
        {
            if (actualExpShakeTimer >= 0.0f)
            {
                actualExpShakeTimer -= Time.deltaTime;
                Vector3 newPos = transform.localPosition + CalculateRandomShake(shakeExpFactor, true);
                transform.localPosition = Vector3.Lerp(transform.localPosition, newPos, shakeExpSmoothness * Time.deltaTime);
            }
            else
            {
                isExpShaking = false;
                actualExpShakeTimer = shakeExpTimer;
            }
        }

        FollowSpeed = Mathf.MoveTowards(FollowSpeed, _targetFollowSpeed, Time.deltaTime);
    }

    public void EnableBlood()
    {
        if (showBlood)
        {
            var newMask = GetComponentInChildren<Camera>().cullingMask & ~(1 << LayerMask.NameToLayer("Blood"));
            //Debug.Log(GetComponentInChildren<Camera>().cullingMask);
            GetComponentInChildren<Camera>().cullingMask = newMask;
            showBlood = false;
        }
        else if (!showBlood)
        {
            var newMask = GetComponentInChildren<Camera>().cullingMask | (1 << LayerMask.NameToLayer("Blood"));
           // Debug.Log(GetComponentInChildren<Camera>().cullingMask);
            GetComponentInChildren<Camera>().cullingMask = newMask;
            showBlood = true;
        }
        
    }

    public void SetTargetFollowSpeed(float targetFollowSpeed)
    {
        _targetFollowSpeed = targetFollowSpeed;
    }
}
