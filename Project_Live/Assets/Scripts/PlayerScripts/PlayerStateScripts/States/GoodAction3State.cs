using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodAction3State : IPlayerState
{
    PlayerAnimationController anim;

    public GoodAction3State(PlayerAnimationController anim)
    {
        this.anim = anim;
    }

    public void Enter()
    {
        Debug.Log("�����˃A�N�V����3��ԂɈڍs");
        anim.PlayGoodAction3();
    }

    public void Update()
    {

    }

    public void Exit()
    {
        Debug.Log("�����˃A�N�V����3��ԂɈڍs");
    }
}
