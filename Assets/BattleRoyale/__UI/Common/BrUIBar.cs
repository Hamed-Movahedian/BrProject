using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BrUIBar : MonoBehaviour
{
    public Slider Slider;

    public UnityEvent OnIncrease;
    public UnityEvent OnDerease;

    public delegate void UpdateBar(int value);

    private float targetValue;
    private float velocity;

    public void OnUpdateBar(int value)
    {
        if (value < targetValue)
        {
            targetValue = value;
            OnDerease.Invoke();
        }
        else if (value > targetValue)
        {
            targetValue = value;
            OnIncrease.Invoke();
        }
    }

    public void Initialize(int value, int max)
    {
        Slider.maxValue = max;
        Slider.value = targetValue = value;
    }

    private void Update()
    {
        if (targetValue == Slider.value)
            return;

        Slider.value = Mathf.SmoothDamp(Slider.value, targetValue, ref velocity, 0.3f);

        if (Math.Abs(targetValue - Slider.value) < 0.3f)
            Slider.value = targetValue;

    }
}