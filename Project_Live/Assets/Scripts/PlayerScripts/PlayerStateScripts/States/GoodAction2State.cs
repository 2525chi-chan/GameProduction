using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//作成者：桑原

public class GoodAction2State : IPlayerState
{
    PlayerAnimationController anim;
    GoodAction goodAction;
    GameObject actionUsedEffect;

    float currentStateTime = 0f;
    bool isActionActivated = false;

    public GoodAction2State(PlayerAnimationController anim, GoodAction goodAction)
    {
        this.anim = anim;
        this.goodAction = goodAction;

        actionUsedEffect = goodAction?.GoodAction1Parameters.GoodActionUsedEffect;
    }

    public void Enter()
    {
        //Debug.Log("いいねアクション2状態に移行");
        anim.PlayGoodAction2();
        actionUsedEffect?.SetActive(true);
    }

    public void Update()
    {
        currentStateTime += Time.deltaTime;

        if (currentStateTime < goodAction.GoodAction2Parameters.ActionInterval) return;

        if (!isActionActivated)
        {
            goodAction.GoodAction2();
            isActionActivated = true;
            actionUsedEffect?.SetActive(false);
        }

        if (currentStateTime < goodAction.GoodAction2Parameters.ChangeStateInterval) return;

        PlayerActionEvents.IdleEvent();
    }

    public void Exit()
    {
        currentStateTime = 0f;
        isActionActivated = false;
        actionUsedEffect?.SetActive(false);
        //Debug.Log("いいねアクション2状態を終了");
    }
}
