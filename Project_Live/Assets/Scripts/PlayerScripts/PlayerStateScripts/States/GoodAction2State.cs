using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

//作成者：桑原

public class GoodAction2State : IPlayerState
{
    PlayerAnimationController anim;
    PlayerStatus status;
    GoodAction goodAction;
    GameObject actionUsedEffect;

    float currentStateTime = 0f;
    bool isActionActivated = false;
    Transform origin;
    public GoodAction2State(PlayerAnimationController anim, PlayerStatus status, GoodAction goodAction)
    {
        this.anim = anim;
        this.status = status;
        this.goodAction = goodAction;
        origin = goodAction.transform;
        actionUsedEffect = goodAction?.GoodAction2Parameters.GoodActionUsedEffect;
    }

    public void Enter()
    {
        //Debug.Log("いいねアクション2状態に移行");
        anim.PlayGoodAction2();

        if (goodAction.GoodAction2Parameters.IsInvincible)
            status.CurrentState = PlayerStatus.PlayerState.Invincible;
        
        GameObject effect = GameObject.Instantiate(actionUsedEffect, origin.position,Quaternion.identity);
        
    }

    public void Update()
    {
        currentStateTime += Time.deltaTime;

        if (currentStateTime < goodAction.GoodAction2Parameters.ActionInterval) return;

        if (!isActionActivated)
        {
            goodAction.GoodAction2();
            isActionActivated = true;
      //      actionUsedEffect?.SetActive(false);
        }
        var animationState = anim.Animator.GetCurrentAnimatorStateInfo(0);
        //if (currentStateTime < goodAction.GoodAction2Parameters.ChangeStateInterval) return;
        if (animationState.normalizedTime >= 1.0f && animationState.IsName("GoodAction2"))
        {
            PlayerActionEvents.IdleEvent();

        }

           
    }

    public void Exit()
    {
        currentStateTime = 0f;
        isActionActivated = false;
        if (goodAction.GoodAction2Parameters.IsInvincible)
            status.CurrentState = PlayerStatus.PlayerState.Normal;
        //  actionUsedEffect?.SetActive(false);
        //Debug.Log("いいねアクション2状態を終了");
    }
}
