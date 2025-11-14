using UnityEngine;

public class DownState_Enemy : IEnemyState
{
    EnemyAnimationController anim;

    public DownState_Enemy(EnemyAnimationController anim)
    {
        this.anim = anim;
    }

    public void Enter()
    {
        //Debug.Log("ダウン状態に移行");
        //anim.PlayDown();
        anim.PlayDamage();
    }

    public void Update()
    {

    }

    public void Exit()
    {
        //Debug.Log("ダウン状態終了");
    }
}
