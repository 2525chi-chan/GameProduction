using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

//�쐬�ҁF�K��

public class GoodAction3State : IPlayerState
{
    PlayerAnimationController anim;
    PlayerStatus status;
    GoodAction goodAction;
    GameObject actionUsedEffect;

    float currentStateTime = 0f;
    bool isActionActivated = false;
    Transform origin;
    public GoodAction3State(PlayerAnimationController anim, PlayerStatus status, GoodAction goodAction)
    {
        this.anim = anim;
        this.status = status;
        this.goodAction = goodAction;
        origin = goodAction.transform;
        actionUsedEffect = goodAction?.GoodAction3Parameters.GoodActionUsedEffect;
    }

    public void Enter()
    {
        //Debug.Log("�����˃A�N�V����3��ԂɈڍs");
        anim.PlayGoodAction3();

        if (goodAction.GoodAction3Parameters.IsInvincible)
            status.CurrentState = PlayerStatus.PlayerState.Invincible;

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
        var animationState = anim.Animator.GetCurrentAnimatorStateInfo(0);
        //  if (currentStateTime < goodAction.GoodAction3Parameters.ChangeStateInterval) return;
        if (animationState.normalizedTime >= 1.0f && animationState.IsName("GoodAction3"))
        {
            PlayerActionEvents.IdleEvent();
        }


    }

    public void Exit()
    {
        currentStateTime = 0f;
        isActionActivated = false;
        if (goodAction.GoodAction3Parameters.IsInvincible)
            status.CurrentState = PlayerStatus.PlayerState.Normal;
        //  actionUsedEffect?.SetActive(false);
        //Debug.Log("�����˃A�N�V����3��Ԃ��I��");
    }
}
