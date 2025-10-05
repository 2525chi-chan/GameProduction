using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

//作成者：桑原

public class GoodAction4State : IPlayerState
{
    PlayerAnimationController anim;
    PlayerStatus status;
    GoodAction goodAction;
    GameObject actionUsedEffect;

    float currentStateTime = 0f;
    bool isActionActivated = false;
    Transform origin;
    public GoodAction4State(PlayerAnimationController anim, PlayerStatus status, GoodAction goodAction)
    {
        this.anim = anim;
        this.status = status;
        this.goodAction = goodAction;
        origin= goodAction.transform;
        actionUsedEffect = goodAction?.GoodAction4Parameters.GoodActionUsedEffect;
    }

    public void Enter()
    {
        //Debug.Log("いいねアクション4状態に移行");
        anim.PlayGoodAction4();

        if (goodAction.GoodAction4Parameters.IsInvincible)
            status.CurrentState = PlayerStatus.PlayerState.Invincible;

        GameObject effect=GameObject.Instantiate(actionUsedEffect,origin.position,Quaternion.identity);
      
    }

    public void Update()
    {
        currentStateTime += Time.deltaTime;

        if (currentStateTime < goodAction.GoodAction4Parameters.ActionInterval) return;

        if (!isActionActivated)
        {
            goodAction.GoodAction4();
            isActionActivated = true;
          //  actionUsedEffect?.SetActive(false);
        }

        if (currentStateTime < goodAction.GoodAction4Parameters.ChangeStateInterval) return;

        PlayerActionEvents.IdleEvent();
    }

    public void Exit()
    {
        currentStateTime = 0f;
        isActionActivated = false;
        if (goodAction.GoodAction4Parameters.IsInvincible)
            status.CurrentState = PlayerStatus.PlayerState.Normal;
        // actionUsedEffect?.SetActive(false);
        //Debug.Log("いいねアクション4状態を終了");
    }
}
