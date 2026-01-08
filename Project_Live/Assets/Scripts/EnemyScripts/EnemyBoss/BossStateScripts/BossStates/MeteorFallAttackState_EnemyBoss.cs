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

        // çUåÇäÆóπîªíË
        if (!isPlayed && meteorFallAttack.CurrentAttackState == MeteorFallAttack_Boss.AttackState.Attack)
        {
            isPlayed = true;
            anim.PlayMeteorFallAttack();
        }
    }

    public void Exit()
    {
        meteorFallAttack.CurrentAttackState = MeteorFallAttack_Boss.AttackState.Exit;
        //Debug.Log("Ë¶êŒóéâ∫çUåÇèIóπ");
    }
}
