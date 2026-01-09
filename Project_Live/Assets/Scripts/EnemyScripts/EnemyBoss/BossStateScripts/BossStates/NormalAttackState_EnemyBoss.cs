using UnityEngine;

public class NormalAttackState_EnemyBoss : IEnemyState
{
    EnemyAnimationController anim;
    EnemyMover mover;
    NormalAttack_Boss normalAttack;

    bool isPlayed;

    public NormalAttackState_EnemyBoss(EnemyAnimationController anim, EnemyMover mover, NormalAttack_Boss normalAttack)
    {
        this.anim = anim;
        this.mover = mover;
        this.normalAttack = normalAttack;
    }

    public void Enter()
    {
        isPlayed = false;
        anim.PlayIdle();
        normalAttack.IsActive = true;
        normalAttack.SetStartState();
        mover.SetMoveType(EnemyMover.EnemyMoveType.PlayerChase);
        //Debug.Log("çUåÇèÛë‘Ç…à⁄çs");
    }

    public void Update()
    {
        normalAttack.StateProcess();

        if (!isPlayed && normalAttack.CurrentAttackState == NormalAttack_Boss.AttackState.Attack)
        {
            isPlayed = true;
            anim.PlayNormalAttack();
            normalAttack.PlayChargeSound();
        }

        var animatorState = anim.Animator.GetCurrentAnimatorStateInfo(0);

        if (animatorState.normalizedTime >= 0.43f && animatorState.IsName("Enemy_Attack_Boss") && !normalAttack.IsAttacked)
        {          
            normalAttack.IsAttacked = true;
            normalAttack.InstanceNormalAttack();
            normalAttack.PlayAttackSound();
        }

        if (animatorState.IsName("Idle") && normalAttack.IsAttacked)
        {
            normalAttack.CurrentAttackState = NormalAttack_Boss.AttackState.Cooldown;
            normalAttack.IsAttacked = false;
            isPlayed = false;
        }
    }

    public void Exit()
    {
        normalAttack.CurrentAttackState = NormalAttack_Boss.AttackState.Exit;
        //Debug.Log("çUåÇèÛë‘èIóπ");
    }
}
