using UnityEngine;

public class MoveState_Enemy : IEnemyState
{
    EnemyAnimationController anim;
    EnemyMover mover;

    public MoveState_Enemy(EnemyAnimationController anim, EnemyMover mover)
    {
        this.anim = anim;
        this.mover = mover;
    }

    public void Enter()
    {
        //Debug.Log("ˆÚ“®ó‘Ô‚ÉˆÚs");
        anim.PlayMove();
    }

    public void Update()
    {
        mover.MoveStateProcess(); //“G‚ÌˆÚ“®ˆ—
    }

    public void Exit()
    {
        //Debug.Log("ˆÚ“®ó‘Ô‚ğI—¹");
    }
}
