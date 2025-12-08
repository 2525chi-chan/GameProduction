using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emote2State : IPlayerState
{
    PlayerAnimationController anim;

    public Emote2State(PlayerAnimationController anim)
    {
        this.anim = anim;
    }

    public void Enter()
    {
        //Debug.Log("エモート2状態に移行");
        anim.PlayEmote2();
    }

    public void Update()
    {
        var animatorState = anim.Animator.GetCurrentAnimatorStateInfo(0);
        if (animatorState.normalizedTime >= 1.0f && animatorState.IsName("Emote2"))
            PlayerActionEvents.IdleEvent();
    }

    public void Exit()
    {
        //Debug.Log("エモート2状態を終了");
    }
}

