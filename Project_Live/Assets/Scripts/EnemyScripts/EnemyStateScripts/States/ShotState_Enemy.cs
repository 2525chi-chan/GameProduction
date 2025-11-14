using UnityEngine;

public class ShotState_Enemy : IEnemyState
{
    EnemyAnimationController anim;

    public ShotState_Enemy(EnemyAnimationController anim)
    {
        this.anim = anim;
    }

    public void Enter()
    {
        Debug.Log("‰“‹——£UŒ‚ó‘Ô‚ÉˆÚs");
        anim.PlayIdle();

    }

    public void Update()
    {

    }

    public void Exit()
    {
        Debug.Log("‰“‹——£UŒ‚ó‘ÔI—¹");
    }
}
