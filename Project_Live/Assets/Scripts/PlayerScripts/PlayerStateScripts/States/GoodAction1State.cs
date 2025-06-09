using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodAction1State : IPlayerState
{
    PlayerAnimationController anim;

    public GoodAction1State(PlayerAnimationController anim)
    {
        this.anim = anim;
    }

    public void Enter()
    {
        Debug.Log("�����˃A�N�V����1��ԂɈڍs");
        anim.PlayGoodAction1();
    }

    public void Update()
    {

    }

    public void Exit()
    {
        Debug.Log("�����˃A�N�V����1��Ԃ��I��");
    }
}

