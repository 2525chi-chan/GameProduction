using UnityEngine;

public class LongRangeAttackState_EnemyBoss : IEnemyState
{
    EnemyAnimationController anim;
    EnemyMover mover;
    LongRangeAttack_Boss longRangeAttack;

    public LongRangeAttackState_EnemyBoss(EnemyAnimationController anim, EnemyMover mover, LongRangeAttack_Boss longRangeAttack)
    {
        this.anim = anim;
        this.mover = mover;
        this.longRangeAttack = longRangeAttack;
    }

    public void Enter()
    {
        longRangeAttack.IsActive = true;
        longRangeAttack.SetStartState();
        longRangeAttack.SetTargetPosition();

        //Debug.Log("çUåÇèÛë‘Ç…à⁄çs");
    }

    public void Update()
    {
        longRangeAttack.StateProcess();
        
        if ((longRangeAttack.CurrentAttackState != longRangeAttack.PreviousAttackState)
            && longRangeAttack.CurrentAttackState == LongRangeAttack_Boss.AttackState.Attack)
            anim.PlayLongRangeAttack();
    }

    public void Exit()
    {
        longRangeAttack.IsActive = false;
        //Debug.Log("çUåÇèÛë‘èIóπ");
    }
}
