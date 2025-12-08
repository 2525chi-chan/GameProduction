using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emote3State : IPlayerState
{
    PlayerAnimationController anim;

    public Emote3State(PlayerAnimationController anim)
    {
        this.anim = anim;
    }

    public void Enter()
    {
        //Debug.Log("エモート3状態に移行");
        anim.PlayEmote3();
    }

    public void Update()
    {
        var animatorState = anim.Animator.GetCurrentAnimatorStateInfo(0);
        if (animatorState.normalizedTime >= 1.0f && animatorState.IsName("Emote3"))
            PlayerActionEvents.IdleEvent();
    }

    public void Exit()
    {
        //Debug.Log("エモート3状態を終了");
    }
}

