﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BrCharacterHitEffect : MonoBehaviour
{
    public SkinnedMeshRenderer renderer;
    public Color hitColor = Color.gray;
    public float duration = 0;
    public float animDuration = 0.25f;
    private Color _defaultColor;
    private Animator animator;
    private bool run;
    private Coroutine corotine;

    public UnityEvent OnHit;
    // Use this for initialization
    void Start()
    {
        
        //_defaultColor = renderer.material.GetColor("_EmissionColor");
        animator = GetComponent<Animator>();

    }
    [ContextMenu("Hit")]
    public void Hit()
    {
        if (run)
        {
            animator.SetTrigger("HitRepeat");
            return;
        }
        run = true;
        corotine = StartCoroutine(HitCorotine());
    }

    private IEnumerator HitCorotine()
    {
        OnHit.Invoke();
        animator.SetBool("Hit", true);
        //renderer.material.SetColor("_EmissionColor", hitColor);
        //yield return new WaitForSeconds(duration);
        //renderer.material.SetColor("_EmissionColor", _defaultColor);
        yield return new WaitForSeconds(animDuration - duration);

        animator.SetBool("Hit", false);

        run = false;
    }

    
    private void endEffect()
    {
    }
}
