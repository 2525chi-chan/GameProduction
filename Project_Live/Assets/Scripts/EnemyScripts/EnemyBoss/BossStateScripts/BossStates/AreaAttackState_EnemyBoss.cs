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

        if (!isPlayed && wideAreaAttack.CurrentAttackState == WideAreaAttack_Boss.AttackState.Attack)
        {
            isPlayed = true;
            anim.PlayAreaAttack();
            wideAreaAttack.PlayChargeSound();
        }

        var animatorState = anim.Animator.GetCurrentAnimatorStateInfo(0);

        if (animatorState.normalizedTime >= 0.44f && animatorState.IsName("Enemy_AreaAttack_Boss") && !wideAreaAttack.IsAttacked)
        {
            wideAreaAttack.IsAttacked = true;
            wideAreaAttack.InstanceWideAreaAttack();
            wideAreaAttack.PlayAttackSound();
        }

        if (animatorState.IsName("Idle") && wideAreaAttack.IsAttacked)
        {
            wideAreaAttack.CurrentAttackState = WideAreaAttack_Boss.AttackState.Cooldown;
            wideAreaAttack.IsAttacked = false;
            isPlayed = false;
        }
    }

    public void Exit()
    {
        wideAreaAttack.CurrentAttackState = WideAreaAttack_Boss.AttackState.Exit;
        //Debug.Log("çUåÇèÛë‘èIóπ");
    }
}
