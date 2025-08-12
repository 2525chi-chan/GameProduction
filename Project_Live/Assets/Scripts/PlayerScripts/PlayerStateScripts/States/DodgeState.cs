using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//作成者：桑原

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
        //Debug.Log("回避状態に移行");
        anim.PlayDodge(); //回避アニメーションの再生
        dodge.TryDodge(); //回避の開始
    }

    public void Update()
    {
        dodge.DodgeProcess(); //回避中の処理
    }

    public void Exit()
    {
        //Debug.Log("回避状態を終了");
    }
}

