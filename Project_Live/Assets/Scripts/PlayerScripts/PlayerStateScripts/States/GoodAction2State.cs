using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodAction2State : IPlayerState
{
    PlayerAnimationController anim;
    GoodAction goodAction;

    public GoodAction2State(PlayerAnimationController anim, GoodAction goodAction)
    {
        this.anim = anim;
        this.goodAction = goodAction;
    }

    public void Enter()
    {
        Debug.Log("�����˃A�N�V������ԂɈڍs");
        anim.PlayGoodAction2();
    }

    public void Update()
    {

    }

    public void Exit()
    {
        Debug.Log("�����˃A�N�V������Ԃ��I��");
    }
}
