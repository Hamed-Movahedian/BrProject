using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrDamageText : MonoBehaviour
{
    public TextMesh text; 
    public TextMesh shadow;
    public Animator animator;

    internal void Initialize(int amount, Color color)
    {
        text.text = amount.ToString();
        shadow.text = amount.ToString();
        text.color = color;
        animator.SetTrigger("Show");
            
    }
    public void Disable()
    {
        BrPoolManager.Instance.Destroy(gameObject);
    }
}
