using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//作成者：桑原

public class GoodAction1State : IPlayerState
{
    PlayerAnimationController anim;
    PlayerStatus status;
    GoodAction goodAction;
    GameObject actionUsedEffect;

    float currentStateTime = 0f;
    bool isActionActivated = false;
    Transform origin;
    public GoodAction1State(PlayerAnimationController anim, PlayerStatus status, GoodAction goodAction)
    {
        this.anim = anim;
        this.status = status;
        this.goodAction = goodAction;
        origin = goodAction.transform;
        actionUsedEffect = goodAction?.GoodAction1Parameters.GoodActionUsedEffect;
    }

    public void Enter()
    {
        //Debug.Log("いいねアクション1状態に移行");
        anim.PlayGoodAction1();

        if (goodAction.GoodAction1Parameters.IsInvincible)
            status.CurrentState = PlayerStatus.PlayerState.Invincible;
        
        GameObject effect = GameObject.Instantiate(actionUsedEffect, origin.position, Quaternion.identity);

    }

    public void Update()
    {
        currentStateTime += Time.deltaTime;

        if (currentStateTime < goodAction.GoodAction1Parameters.ActionInterval) return;

        if (!isActionActivated)
        {
            goodAction.GoodAction1();
            isActionActivated = true;
         //   actionUsedEffect?.SetActive(false);
        }

        if (isActionActivated)
        {

        }

        if (currentStateTime < goodAction.GoodAction1Parameters.ChangeStateInterval) return;

        PlayerActionEvents.IdleEvent();
    }

    public void Exit()
    {
        currentStateTime = 0f;
        isActionActivated = false;
        if (goodAction.GoodAction1Parameters.IsInvincible)
            status.CurrentState = PlayerStatus.PlayerState.Normal;
        //  actionUsedEffect?.SetActive(false);
        //Debug.Log("いいねアクション1状態を終了");
    }
}

