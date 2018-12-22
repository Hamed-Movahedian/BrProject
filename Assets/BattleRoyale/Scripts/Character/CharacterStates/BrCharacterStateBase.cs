
using System;
using UnityEngine;

public abstract class BrCharacterStateBase
{
    protected BrCharacterController _controller;

    public void Initialize(BrCharacterController characterController)
    {
        _controller = characterController;
    }

    public virtual void Start(){}

    public virtual void Update(){}

    public virtual void FixedUpdate(){}
    
    public virtual void OnEnter(){}
    public virtual void OnExit(){}
}