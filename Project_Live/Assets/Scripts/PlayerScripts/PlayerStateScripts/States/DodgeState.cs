using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�쐬�ҁF�K��

public class DodgeState : IPlayerState
{
    PlayerAnimationController anim;
    MovePlayer movePlayer;
    Dodge dodge;

    public DodgeState(PlayerAnimationController anim, MovePlayer movePlayer, Dodge dodge)
    {
        this.anim = anim;
        this.movePlayer = movePlayer;
        this.dodge = dodge;
    }

    public void Enter()
    {
        //Debug.Log("�����ԂɈڍs");
        anim.PlayDodge(); //����A�j���[�V�����̍Đ�
        dodge.TryDodge(); //����̊J�n
    }

    public void Update()
    {
        dodge.DodgeProcess(); //��𒆂̏���
    }

    public void Exit()
    {
        //Debug.Log("�����Ԃ��I��");
    }
}

