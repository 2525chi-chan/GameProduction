using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

//�쐬�ҁF�K��

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
        //Debug.Log("�����˃A�N�V����3��ԂɈڍs");
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
        //Debug.Log("�����˃A�N�V����3��Ԃ��I��");
    }
}
