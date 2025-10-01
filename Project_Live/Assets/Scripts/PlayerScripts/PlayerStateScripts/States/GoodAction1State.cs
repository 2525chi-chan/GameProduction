using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�쐬�ҁF�K��

public class GoodAction1State : IPlayerState
{
    PlayerAnimationController anim;
    GoodAction goodAction;
    GameObject actionUsedEffect;

    float currentStateTime = 0f;
    bool isActionActivated = false;
    Transform origin;
    public GoodAction1State(PlayerAnimationController anim, GoodAction goodAction)
    {
        this.anim = anim;
        this.goodAction = goodAction;
        origin = goodAction.transform;
        actionUsedEffect = goodAction?.GoodAction1Parameters.GoodActionUsedEffect;
    }

    public void Enter()
    {
        //Debug.Log("�����˃A�N�V����1��ԂɈڍs");
        anim.PlayGoodAction1();
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

        if (currentStateTime < goodAction.GoodAction1Parameters.ChangeStateInterval) return;

        PlayerActionEvents.IdleEvent();
    }

    public void Exit()
    {
        currentStateTime = 0f;
        isActionActivated = false;
      //  actionUsedEffect?.SetActive(false);
        //Debug.Log("�����˃A�N�V����1��Ԃ��I��");
    }
}

