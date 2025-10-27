using UnityEngine;

public class AttackState_Enemy : IEnemyState
{
    EnemyAnimationController anim;
    EnemyMover mover;
    AttackController attackController;

    float timer = 0f; //攻撃待機時間の計測用変数

    public AttackState_Enemy(EnemyAnimationController anim, EnemyMover mover, AttackController attackController)
    {
        this.anim = anim;
        this.mover = mover;
        this.attackController = attackController;
    }

    public void Enter()
    {
        Debug.Log("近接攻撃状態に移行");
        anim.PlayCloseAttack(); //近接攻撃アニメーションの再生

    }

    public void Update()
    {
        mover.MoveStateProcess(); //移動処理（プレイヤーの注視、回転動作など）

        timer += Time.deltaTime;

        if (timer > attackController.AttackDuration)
        {
            timer = 0f;
            attackController.InstanceAttack();
        }
    }

    public void Exit()
    {
        Debug.Log("近接攻撃状態終了");
    }
}
