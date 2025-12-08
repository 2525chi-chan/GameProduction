using UnityEngine;

public class AreaAttackState_EnemyBoss : IEnemyState
{
    EnemyAnimationController anim;
    EnemyMover mover;
    WideAreaAttack_Boss wideAreaAttack;

    public AreaAttackState_EnemyBoss(EnemyAnimationController anim, EnemyMover mover, WideAreaAttack_Boss wideAreaAttack)
    {
        this.anim = anim;
        this.mover = mover;
        this.wideAreaAttack = wideAreaAttack;
    }

    public void Enter()
    {
        wideAreaAttack.IsActive = true;
        wideAreaAttack.SetStartState();
        mover.SetMoveType(EnemyMover.EnemyMoveType.PlayerChase);
        //Debug.Log("UŒ‚ó‘Ô‚ÉˆÚs");
    }

    public void Update()
    {
        wideAreaAttack.StateProcess();
        mover.MoveStateProcess();

        if ((wideAreaAttack.CurrentAttackState != wideAreaAttack.PreviousAttackState)
            && wideAreaAttack.CurrentAttackState == WideAreaAttack_Boss.AttackState.Attack)
            anim.PlayAreaAttack();
    }

    public void Exit()
    {
        wideAreaAttack.IsActive = false;
        //Debug.Log("UŒ‚ó‘ÔI—¹");
    }
}
