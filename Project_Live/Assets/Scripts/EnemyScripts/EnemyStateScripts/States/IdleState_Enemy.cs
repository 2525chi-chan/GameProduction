using Unity.VisualScripting;
using UnityEngine;

class IdleState_Enemy : IEnemyState
{
    EnemyActionStateMachine stateMachine;
    EnemyAnimationController anim;
    EnemyStatus status;
    EnemyMover mover;

    public IdleState_Enemy(EnemyActionStateMachine stateMachine, EnemyAnimationController anim, EnemyStatus status, EnemyMover mover)
    {
        this.stateMachine = stateMachine;
        this.anim = anim;
        this.status = status;
        this.mover = mover;
    }

    public void Enter()
    {
        //Debug.Log("待機状態に移行");
    }

    public void Update()
    {
        //mover.MoveStateProcess();
        if (stateMachine.IsBossBehaviour)
            stateMachine.actionEvents.BossAttackStartEvent();
    }

    public void Exit()
    {
        //Debug.Log("待機状態終了");
    }
}
