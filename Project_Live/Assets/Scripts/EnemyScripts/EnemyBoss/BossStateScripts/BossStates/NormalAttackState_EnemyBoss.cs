using UnityEngine;

public class NormalAttackState_EnemyBoss : IEnemyState
{
    EnemyAnimationController anim;
    EnemyMover mover;
    NormalAttack_Boss normalAttack;

    public NormalAttackState_EnemyBoss(EnemyAnimationController anim, EnemyMover mover, NormalAttack_Boss normalAttack)
    {
        this.anim = anim;
        this.mover = mover;
        this.normalAttack = normalAttack;
    }

    public void Enter()
    {
        normalAttack.IsActive = true;
        normalAttack.SetStartState();
        mover.SetMoveType(EnemyMover.EnemyMoveType.PlayerChase);
        //Debug.Log("çUåÇèÛë‘Ç…à⁄çs");
    }

    public void Update()
    {
        normalAttack.StateProcess();
        mover.MoveStateProcess();

        if ((normalAttack.CurrentAttackState != normalAttack.PreviousAttackState)
            && normalAttack.CurrentAttackState == NormalAttack_Boss.AttackState.Attack)
            anim.PlayNormalAttack();
    }

    public void Exit()
    {
        normalAttack.IsActive = false;
        //Debug.Log("çUåÇèÛë‘èIóπ");
    }
}
