using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�쐬�ҁF�K�����

public class IdleState : IPlayerState
{
    PlayerAnimationController anim;

    public IdleState(PlayerAnimationController anim)
    {
        if (anim == null)
        {
            Debug.Log("error");
            return;
        }

        this.anim = anim;
    }

    public void Enter()
    {
        if (anim == null)
        {
            Debug.Log("missEnter");
            return;
        }
        //Debug.Log("�ҋ@��ԂɈڍs");
        anim.PlayIdle();

    }

    public void Update()
    {

    }

    public void Exit()
    {
        //Debug.Log("�ҋ@��ԏI��");
    }
}
