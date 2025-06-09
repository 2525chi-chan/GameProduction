using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodAction4State : IPlayerState
{
    PlayerAnimationController anim;

    public GoodAction4State(PlayerAnimationController anim)
    {
        this.anim = anim;
    }

    public void Enter()
    {
        Debug.Log("�����˃A�N�V����4��ԂɈڍs");
        anim.PlayGoodAction4();
    }

    public void Update()
    {

    }

    public void Exit()
    {
        Debug.Log("�����˃A�N�V����4��Ԃ��I��");
    }
}
