using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    public EnemyActionEvents actionEvents = new EnemyActionEvents();

    [Header("攻撃を始める判定を行う領域")]
    [SerializeField] SphereCollider triggerRange;
    
    [Header("攻撃を行うまでに必要な判定時間")]
    [SerializeField] float triggerDuration = 0.8f;

    [Header("必要なコンポーネント")]
    [SerializeField] EnemyStatus status;
    [SerializeField] EnemyMover mover;
    [SerializeField] Animator animator;

    float currentTimer = 0f; //経過時間の測定用
    bool isAttackTrigger = false; //攻撃するかどうか
    bool isInRange = false;

    public bool IsAttackTrigger { get { return isAttackTrigger; } set { isAttackTrigger = value; } }

    void OnTriggerEnter(Collider other) //攻撃判定を行うエリアにプレイヤーが侵入しているときの処理
    {
        if (status.IsDead || isAttackTrigger == true) return; //敵のHPが0、または攻撃の処理を開始している場合は何もしない

        if (!IsValidTarget(other)) return;

        isInRange = true;
        currentTimer = 0f;             
    }

    void Update()
    {
        if (!isInRange || status.IsDead || isAttackTrigger) return;

        currentTimer += Time.deltaTime;

        if (currentTimer > triggerDuration && !isAttackTrigger)
        {
            isAttackTrigger = true;
            actionEvents.AttackEvent();
        }
    }

    void OnTriggerExit(Collider other)　//攻撃判定を行うエリアからプレイヤーが出たときの処理
    {
        if (!IsValidTarget(other)) return;
        currentTimer = 0f;
        isInRange = false;
        isAttackTrigger = false;
        actionEvents.IdleEvent();
    }

    bool IsValidTarget(Collider other)
    {
        switch (mover.MoveType)
        {
            case EnemyMover.EnemyMoveType.PlayerChase:
            case EnemyMover.EnemyMoveType.BlockPlayer:
                return other.CompareTag("Player");

            case EnemyMover.EnemyMoveType.StageDestroy:
                return other.CompareTag("Breakable");

            default: return false;
        }
    }

}
