using UnityEngine;

public class MeteorFallAttackState_EnemyBoss : IEnemyState
{
    EnemyAnimationController anim;
    EnemyMover mover;
    MeteorFallAttack_Boss meteorFallAttack;

    bool isPlayed;

    public MeteorFallAttackState_EnemyBoss(EnemyAnimationController anim, EnemyMover mover, MeteorFallAttack_Boss meteorFallAttack)
    {
        this.anim = anim;
        this.mover = mover;
        this.meteorFallAttack = meteorFallAttack;
    }

    public void Enter()
    {
        isPlayed = false;
        anim.PlayIdle();
        meteorFallAttack.SetTargetPosition();
        meteorFallAttack.IsActive = true;
        meteorFallAttack.SetStartState();

        //Debug.Log("Ë¶êŒóéâ∫çUåÇäJén");
    }

    public void Update()
    {
        meteorFallAttack.StateProcess();

        if (meteorFallAttack.TargetPosition == null)
            meteorFallAttack.SetTargetPosition();

        if (!isPlayed && meteorFallAttack.CurrentAttackState == MeteorFallAttack_Boss.AttackState.Attack)
        {
            isPlayed = true;
            anim.PlayMeteorFallAttack();
            meteorFallAttack.PlayChargeSound();
        }

        var animatorState = anim.Animator.GetCurrentAnimatorStateInfo(0);

        if (animatorState.normalizedTime >= 0.29f && animatorState.IsName("Enemy_MeteorFallAttack_Boss") && !meteorFallAttack.IsAttacked)
        {
            meteorFallAttack.IsAttacked = true;
            meteorFallAttack.StartFirstMeteorFall();           
            meteorFallAttack.PlayAttackSound();
        }

        if (animatorState.IsName("Idle") && meteorFallAttack.IsAttacked)
        {
            meteorFallAttack.CurrentAttackState = MeteorFallAttack_Boss.AttackState.Cooldown;
            meteorFallAttack.IsAttacked = false;
            isPlayed = false;
        }
    }

    public void Exit()
    {
        meteorFallAttack.CurrentAttackState = MeteorFallAttack_Boss.AttackState.Exit;
        //Debug.Log("Ë¶êŒóéâ∫çUåÇèIóπ");
    }
}
