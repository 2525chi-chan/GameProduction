using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emote1State : IPlayerState
{
    PlayerAnimationController anim;

    public Emote1State(PlayerAnimationController anim)
    {
        this.anim = anim;
    }

    public void Enter()
    {
        //Debug.Log("エモート1状態に移行");
        anim.PlayEmote1();
    }

    public void Update()
    {
        var animatorState = anim.Animator.GetCurrentAnimatorStateInfo(0);
        if (animatorState.normalizedTime >= 1.0f && animatorState.IsName("Emote1"))
            PlayerActionEvents.IdleEvent();
    }

    public void Exit()
    {
        //Debug.Log("エモート1状態を終了");
    }
}

