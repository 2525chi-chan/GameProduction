using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : IPlayerState
{
    PlayerAnimationController anim;
    MovePlayer movePlayer;

    public MoveState(PlayerAnimationController anim, MovePlayer movePlayer)
    {
        this.anim = anim;
        this.movePlayer = movePlayer;
    }

    public void Enter()
    {
        anim.PlayMove();
        Debug.Log("�ړ���ԂɈڍs");
    }

    public void Update()
    {
        movePlayer.MoveProcess(); //�ړ��̏���
    }

    public void Exit()
    {
        anim.PlayIdle();
        Debug.Log("�ړ���Ԃ��I��");
    }
}
