using Unity.VisualScripting;
using UnityEngine;

class IdleState_Enemy : IEnemyState
{
    EnemyAnimationController anim;
    EnemyStatus status;
    EnemyMover mover;

    public IdleState_Enemy(EnemyAnimationController anim, EnemyStatus status, EnemyMover mover)
    {
        this.anim = anim;
        this.status = status;
        this.mover = mover;
    }

    public void Enter()
    {
        Debug.Log("待機状態に移行");
        anim.PlayIdle();
    }

    public void Update()
    {
        //mover.MoveStateProcess();
    }

    public void Exit()
    {
        Debug.Log("待機状態終了");
    }
}
