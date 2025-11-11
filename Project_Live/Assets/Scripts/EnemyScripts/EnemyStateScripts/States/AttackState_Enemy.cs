using UnityEngine;

public class AttackState_Enemy : IEnemyState
{
    EnemyAnimationController anim;
    EnemyMover mover;
    AttackController attackController;

    float durationTimer = 0f; //攻撃待機時間の計測用変数
    float coolTimer = 0f;
    bool isAnimPlayed = false;
    bool isAttacked = false;

    public AttackState_Enemy(EnemyAnimationController anim, EnemyMover mover, AttackController attackController)
    {
        this.anim = anim;
        this.mover = mover;
        this.attackController = attackController;
    }

    public void Enter()
    {
        Debug.Log("近接攻撃状態に移行");
    }

    public void Update()
    {
        mover.MoveStateProcess(); //移動処理（プレイヤーの注視、回転動作など）

        durationTimer += Time.deltaTime;
        Debug.Log("攻撃状態");

        if (!isAttacked) //攻撃判定発生前の処理
        {
            if (!isAnimPlayed)
            {
                isAnimPlayed = true;
                anim.PlayCloseAttack(); //近接攻撃アニメーションの再生
            }

            if (durationTimer > attackController.AttackDuration && !isAttacked)
            {
                durationTimer = 0f;
                attackController.InstanceAttack();
                isAttacked = true;
            }
        }        

        if (isAttacked) //攻撃判定発生後のクールタイム時間
        {
            coolTimer += Time.deltaTime;
            if (coolTimer > attackController.AttackCoolTime)
            {
                coolTimer = 0f;
                isAnimPlayed = false;
                isAttacked = false;
            }
        }
    }

    public void Exit()
    {
        Debug.Log("近接攻撃状態終了");
    }
}
