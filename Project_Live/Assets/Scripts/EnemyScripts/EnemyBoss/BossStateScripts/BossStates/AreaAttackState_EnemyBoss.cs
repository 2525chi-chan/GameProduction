using UnityEngine;

public class AreaAttackState_EnemyBoss : IEnemyState
{
    EnemyAnimationController anim;
    EnemyMover mover;
    WideAreaAttack_Boss wideAreaAttack;

    bool isPlayed;

    public AreaAttackState_EnemyBoss(EnemyAnimationController anim, EnemyMover mover, WideAreaAttack_Boss wideAreaAttack)
    {
        this.anim = anim;
        this.mover = mover;
        this.wideAreaAttack = wideAreaAttack;
    }

    public void Enter()
    {
        isPlayed = false;
        anim.PlayIdle();
        wideAreaAttack.IsActive = true;
        wideAreaAttack.SetStartState();
        mover.SetMoveType(EnemyMover.EnemyMoveType.PlayerChase);
        //Debug.Log("çUåÇèÛë‘Ç…à⁄çs");
    }

    public void Update()
    {
        wideAreaAttack.StateProcess();
        //mover.MoveStateProcess();

        if (!isPlayed && wideAreaAttack.CurrentAttackState == WideAreaAttack_Boss.AttackState.Attack)
        {
            isPlayed = true;
            anim.PlayNormalAttack();
        }
    }

    public void Exit()
    {
        wideAreaAttack.CurrentAttackState = WideAreaAttack_Boss.AttackState.Exit;
        //Debug.Log("çUåÇèÛë‘èIóπ");
    }
}
