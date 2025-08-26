using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//作成者：桑原

public class GoodAction4State : IPlayerState
{
    PlayerAnimationController anim;
    GoodAction goodAction;
    GameObject actionUsedEffect;

    float currentStateTime = 0f;
    bool isActionActivated = false;

    public GoodAction4State(PlayerAnimationController anim, GoodAction goodAction)
    {
        this.anim = anim;
        this.goodAction = goodAction;

        actionUsedEffect = goodAction?.GoodAction1Parameters.GoodActionUsedEffect;
    }

    public void Enter()
    {
        //Debug.Log("いいねアクション4状態に移行");
        anim.PlayGoodAction4();
        actionUsedEffect?.SetActive(true);
    }

    public void Update()
    {
        currentStateTime += Time.deltaTime;

        if (currentStateTime < goodAction.GoodAction4Parameters.ActionInterval) return;

        if (!isActionActivated)
        {
            goodAction.GoodAction4();
            isActionActivated = true;
            actionUsedEffect?.SetActive(false);
        }

        if (currentStateTime < goodAction.GoodAction4Parameters.ChangeStateInterval) return;

        PlayerActionEvents.IdleEvent();
    }

    public void Exit()
    {
        currentStateTime = 0f;
        isActionActivated = false;
        actionUsedEffect?.SetActive(false);
        //Debug.Log("いいねアクション4状態を終了");
    }
}
