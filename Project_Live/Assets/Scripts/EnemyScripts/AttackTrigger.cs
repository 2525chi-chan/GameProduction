using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//特定のオブジェクトとのみ当たり判定を行うようにする

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

    float currentTimer = 0f; //経過時間の測定用
    bool isAttackTrigger = false; //攻撃するかどうか
    bool isInRange = false;

    public bool IsAttackTrigger { get { return isAttackTrigger; } set { isAttackTrigger = value; } }

    void OnTriggerEnter(Collider other)
    {
        if (!IsValidTarget(other)) return;
        //Debug.Log(other.name + "は有効なターゲットです。攻撃処理を開始します。");
       isInRange = true;
        currentTimer = 0f;
    }

    void OnTriggerExit(Collider other)
    {
        if (!IsValidTarget(other)) return;
        //Debug.Log(other.name + "がトリガー範囲内から出ました");
        currentTimer = 0f;
        isInRange = false;
        isAttackTrigger = false;
        actionEvents.IdleEvent();
    }

    void Update()
    {
        if (!isInRange || status.IsDead || isAttackTrigger) return;

        currentTimer += Time.deltaTime;

        if (currentTimer > triggerDuration && !isAttackTrigger)
        {
            isAttackTrigger = true;
            actionEvents.CloseAttackEvent();
        }
    }

    bool IsValidTarget(Collider other) //当たり判定を行うタグと対象のタグ確認
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
