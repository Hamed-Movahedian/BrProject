using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class BrCameraShaker : MonoBehaviour
{
    public static BrCameraShaker instance;


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
    private Vector3 randomShakePos;

    [Header("Falling")]
    public float FallingShakeFactor = 3f;
    public float FallingShakeSmoothness = 5f;
    private bool fallingShake;

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
        if (fallingShake)
        {
            //transform.localPosition = transform.localPosition + CalculateRandomShake(FallingShakeFactor, false);
            Vector3 newPos = transform.localPosition + CalculateRandomShake(FallingShakeFactor, false);
            transform.localPosition = Vector3.Lerp(transform.localPosition, newPos, FallingShakeSmoothness * Time.deltaTime);

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

        transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, 1 * Time.deltaTime);
    }
    // Use this for initialization
    void Start ()
    {
        BrPlayerTracker.Instance.OnPlayerRegisterd += PlayerRegister;
	}

    private void Awake()
    {
        instance = this;
    }

    private void PlayerRegister(BrCharacterController player)
    {
        if(player.isMine)
        {
            player.FallingState.OnStartFalling.AddListener(() =>
            {
                fallingShake = true;
            });

            player.ParachuteState.OnOpenPara.AddListener(() =>
            {
                fallingShake = false;
            });
        }
    }

}
