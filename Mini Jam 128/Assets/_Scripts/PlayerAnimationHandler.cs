using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    private SpriteRenderer _sr;
    private Animator _animator;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    public void SetHorizontalMovement(float x, float yVel)
    {
        _animator.SetFloat("HorizontalAxis", x);
        _animator.SetFloat("VerticalVelocity", yVel);
    }

    public void SetTrigger(string trigger)
    {
        _animator.SetTrigger(trigger);
    }

    public void SetBool(string boolToSet, bool value)
    {
        _animator.SetBool(boolToSet, value);
    }

    public void Flip(int side)
    {
        bool state = (side == 1) ? false : true;
        _sr.flipX = state;
    }
}
