using UnityEngine;

public class KnockbackState_Enemy : IEnemyState
{
    EnemyAnimationController anim;

    public KnockbackState_Enemy(EnemyAnimationController anim)
    {
        this.anim = anim;
    }

    public void Enter()
    {
        Debug.Log("のけぞり状態に移行");
        anim.PlayDamage(); //のけぞりアニメーションの再生

    }

    public void Update()
    {

    }

    public void Exit()
    {
        Debug.Log("のけぞり状態終了");
    }
}
