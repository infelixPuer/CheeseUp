using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerAnimationState
{
    Idle,
    Run,
    Jump,
    Fall
}

public class PlayerAnimation : MonoBehaviour
{
    private Rigidbody _rb;
    private Animator _anim;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
    }

    public void SetAnimationState(PlayerAnimationState state)
    {
        _anim.SetInteger("State", (int)state);
    }
}