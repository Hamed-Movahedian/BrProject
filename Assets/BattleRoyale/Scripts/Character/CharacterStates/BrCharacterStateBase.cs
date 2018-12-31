
using System;
using UnityEngine;

public abstract class BrCharacterStateBase
{
    protected BrCharacterController _controller;
    protected bool isMine => _controller.IsMine;
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