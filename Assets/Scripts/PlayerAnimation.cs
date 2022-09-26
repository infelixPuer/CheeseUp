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
    private Animator _anim;

    private void Awake() => _anim = GetComponent<Animator>();

    public void SetAnimationState(PlayerAnimationState state) => _anim.SetInteger("State", (int)state);
}