using UnityEngine;

public class KnockbackState_Enemy : IEnemyState
{
    EnemyActionStateMachine stateMachine;
    EnemyAnimationController anim;
    EnemyStatus status;

    float currentTimer;

    public KnockbackState_Enemy(EnemyActionStateMachine stateMachine, EnemyAnimationController anim, EnemyStatus status)
    {
        this.stateMachine = stateMachine;
        this.anim = anim;
        this.status = status;
    }

    public void Enter()
    {
        //Debug.Log("のけぞり状態に移行");
        currentTimer = 0f;
        anim.PlayDamage(); //のけぞりアニメーションの再生

    }

    public void Update()
    {
        currentTimer += Time.deltaTime;

        if (currentTimer >= status.KnockbackRecoveryTime)
            stateMachine.actionEvents.IdleEvent();

    }

    public void Exit()
    {
        //Debug.Log("のけぞり状態終了");
        currentTimer = 0f;
    }
}
