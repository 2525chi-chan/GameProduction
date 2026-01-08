using UnityEngine;

public class LostState_Enemy : IEnemyState
{
    EnemyAnimationController anim;

    public LostState_Enemy(EnemyAnimationController anim)
    {
        this.anim = anim;
    }

    public void Enter()
    {
        Debug.Log("”s–kó‘Ô‚ÉˆÚs");
        anim.PlayLost();
        anim.LockAnyState();
    }

    public void Update()
    {
        
    }

    public void Exit()
    {
        Debug.Log("”s–kó‘ÔI—¹");
    }
}
