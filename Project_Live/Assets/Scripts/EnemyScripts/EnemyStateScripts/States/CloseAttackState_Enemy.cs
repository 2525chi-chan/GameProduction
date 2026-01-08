using UnityEngine;

public class CloseAttackState_Enemy : IEnemyState
{
    EnemyAnimationController anim;
    EnemyMover mover;
    AttackController attackController;

    float durationTimer;
    float coolTimer;
    bool isPlayedAnim;
    bool isAttacked;

    public CloseAttackState_Enemy(EnemyAnimationController anim, EnemyMover mover, AttackController attackController)
    {
        this.anim = anim;
        this.mover = mover;
        this.attackController = attackController;
    }

    public void Enter()
    {
        durationTimer = 0f;
        coolTimer = 0f;
        isPlayedAnim = false;
        isAttacked = false;
        //Debug.Log("近接攻撃状態に移行");
    }

    public void Update()
    {
        if (mover.enemyStatus.ISBossSpawn) return;

        mover.MoveStateProcess(); //移動処理（プレイヤーの注視、回転動作など）

        if (!isAttacked) durationTimer += Time.deltaTime;
        else coolTimer += Time.deltaTime;

        if (!isPlayedAnim)
        {
            anim.PlayAttack();
            isPlayedAnim = true;
        }

        if (durationTimer >= attackController.AttackDuration && isPlayedAnim)
        {
            durationTimer = 0f;
            attackController.InstanceAttack();
            isAttacked = true;
        }

        if (coolTimer >= attackController.AttackCoolTime && isPlayedAnim)
        {
            coolTimer = 0f;
            isPlayedAnim = false;
            isAttacked = false;
        }
    }

    public void Exit()
    {
        //Debug.Log("近接攻撃状態終了");
    }
}
