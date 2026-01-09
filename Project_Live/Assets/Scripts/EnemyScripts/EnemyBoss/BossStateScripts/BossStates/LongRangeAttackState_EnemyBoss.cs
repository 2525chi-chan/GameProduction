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
        //Debug.Log("攻撃状態に移行");
    }

    public void Update()
    {
        longRangeAttack.StateProcess(); //遠距離攻撃の処理

        if (!isPlayed && longRangeAttack.CurrentAttackState == LongRangeAttack_Boss.AttackState.Attack)
        {
            isPlayed = true;
            anim.PlayLongRangeAttack();
            longRangeAttack.PlayChargeSound();
        }

        var animatorState = anim.Animator.GetCurrentAnimatorStateInfo(0);

        //アニメーションの再生時間に合わせて攻撃を生成する
        if (animatorState.normalizedTime >= 0.46f && animatorState.IsName("Enemy_LongRangeAttack_Boss") && !longRangeAttack.IsAttacked)
        {
            longRangeAttack.IsAttacked = true;
            longRangeAttack.InstanceLongRangeAttack();
            longRangeAttack.PlayAttackSound();
        }

        //アニメーションが待機モーションに遷移したら
        if (animatorState.IsName("Idle") && longRangeAttack.IsAttacked)
        {
            longRangeAttack.CurrentAttackState = LongRangeAttack_Boss.AttackState.Cooldown;
            longRangeAttack.IsAttacked = false;
            isPlayed = false;
        }
    }

    public void Exit()
    {
        longRangeAttack.CurrentAttackState = LongRangeAttack_Boss.AttackState.Exit;
        //Debug.Log("攻撃状態終了");
    }
}
