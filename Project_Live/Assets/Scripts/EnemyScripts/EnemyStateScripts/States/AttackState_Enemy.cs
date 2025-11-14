using UnityEngine;

public class AttackState_Enemy : IEnemyState
{
    EnemyAnimationController anim;
    EnemyMover mover;
    AttackController attackController;

    float durationTimer;
    float coolTimer;
    bool isPlayedAnim;
    bool isAttacked;

    public AttackState_Enemy(EnemyAnimationController anim, EnemyMover mover, AttackController attackController)
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
        //Debug.Log("攻撃状態に移行");
    }

    public void Update()
    {
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
        //Debug.Log("攻撃状態終了");
    }
}
