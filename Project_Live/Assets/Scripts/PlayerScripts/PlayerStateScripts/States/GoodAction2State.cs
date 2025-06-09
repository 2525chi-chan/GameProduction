using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodAction2State : IPlayerState
{
    PlayerAnimationController anim;

    public GoodAction2State(PlayerAnimationController anim)
    {
        this.anim = anim;
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
