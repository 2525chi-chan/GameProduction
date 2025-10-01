using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

//�쐬�ҁF�K��

public class GoodAction2State : IPlayerState
{
    PlayerAnimationController anim;
    GoodAction goodAction;
    GameObject actionUsedEffect;

    float currentStateTime = 0f;
    bool isActionActivated = false;
    Transform origin;
    public GoodAction2State(PlayerAnimationController anim, GoodAction goodAction)
    {
        this.anim = anim;
        this.goodAction = goodAction;
        origin = goodAction.transform;
        actionUsedEffect = goodAction?.GoodAction2Parameters.GoodActionUsedEffect;
    }

    public void Enter()
    {
        //Debug.Log("�����˃A�N�V����2��ԂɈڍs");
        anim.PlayGoodAction2();
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

        if (currentStateTime < goodAction.GoodAction2Parameters.ChangeStateInterval) return;

        PlayerActionEvents.IdleEvent();
    }

    public void Exit()
    {
        currentStateTime = 0f;
        isActionActivated = false;
      //  actionUsedEffect?.SetActive(false);
        //Debug.Log("�����˃A�N�V����2��Ԃ��I��");
    }
}
