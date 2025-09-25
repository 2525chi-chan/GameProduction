using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

//作成者：桑原

public class GoodAction3State : IPlayerState
{
    PlayerAnimationController anim;
    GoodAction goodAction;
    GameObject actionUsedEffect;

    float currentStateTime = 0f;
    bool isActionActivated = false;
    Transform origin;
    public GoodAction3State(PlayerAnimationController anim, GoodAction goodAction)
    {
        this.anim = anim;
        this.goodAction = goodAction;
        origin = goodAction.transform;
        actionUsedEffect = goodAction?.GoodAction3Parameters.GoodActionUsedEffect;
    }

    public void Enter()
    {
        //Debug.Log("いいねアクション3状態に移行");
        anim.PlayGoodAction3();
        GameObject effect = GameObject.Instantiate(actionUsedEffect, origin.position, Quaternion.identity);
      
    }

    public void Update()
    {
        currentStateTime += Time.deltaTime;

        if (currentStateTime < goodAction.GoodAction3Parameters.ActionInterval) return;

        if (!isActionActivated)
        {
            goodAction.GoodAction3();
            isActionActivated = true;
         //   actionUsedEffect?.SetActive(false);
        }

        if (currentStateTime < goodAction.GoodAction3Parameters.ChangeStateInterval) return;

        PlayerActionEvents.IdleEvent();
    }

    public void Exit()
    {
        currentStateTime = 0f;
        isActionActivated = false;
      //  actionUsedEffect?.SetActive(false);
        //Debug.Log("いいねアクション3状態を終了");
    }
}
