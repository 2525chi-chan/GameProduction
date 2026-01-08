using UnityEngine;

public class LongRangeAttackState_EnemyBoss : IEnemyState
{
    EnemyAnimationController anim;
    EnemyMover mover;
    LongRangeAttack_Boss longRangeAttack;

    bool isPlayed;

    public LongRangeAttackState_EnemyBoss(EnemyAnimationController anim, EnemyMover mover, LongRangeAttack_Boss longRangeAttack)
    {
        this.anim = anim;
        this.mover = mover;
        this.longRangeAttack = longRangeAttack;
    }

    public void Enter()
    {
        isPlayed = false;
        anim.PlayIdle();
        longRangeAttack.IsActive = true;
        longRangeAttack.SetStartState();
        longRangeAttack.SetTargetPosition();

        //Debug.Log("çUåÇèÛë‘Ç…à⁄çs");
    }

    public void Update()
    {
        longRangeAttack.StateProcess();

        if (!isPlayed && longRangeAttack.CurrentAttackState == LongRangeAttack_Boss.AttackState.Attack)
        {
            isPlayed = true;
            anim.PlayLongRangeAttack();
        }
    }

    public void Exit()
    {
        longRangeAttack.CurrentAttackState = LongRangeAttack_Boss.AttackState.Exit;
        //Debug.Log("çUåÇèÛë‘èIóπ");
    }
}
