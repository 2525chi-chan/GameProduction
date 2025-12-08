using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emote4State : IPlayerState
{
    PlayerAnimationController anim;

    public Emote4State(PlayerAnimationController anim)
    {
        this.anim = anim;
    }

    public void Enter()
    {
        //Debug.Log("エモート4状態に移行");
        anim.PlayEmote4();
    }

    public void Update()
    {
        var animatorState = anim.Animator.GetCurrentAnimatorStateInfo(0);
        if (animatorState.normalizedTime >= 1.0f && animatorState.IsName("Emote4"))
            PlayerActionEvents.IdleEvent();
    }

    public void Exit()
    {
        //Debug.Log("エモート4状態を終了");
    }
}

