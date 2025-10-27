using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
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
        if (IsValidTarget(other))
        {
            isInRange = true;
            currentTimer = 0f;
            Debug.Log("攻撃範囲に入った");
        }        
    }

    void OnTriggerExit(Collider other)
    {
        if (IsValidTarget(other))
        {
            currentTimer = 0f;
            isInRange = false;            
            isAttackTrigger = false;
            Debug.Log("攻撃判定範囲を出た");
            EnemyActionEvents.IdleEvent();
        }
    }

    void Update()
    {
        if (!isInRange || status.IsDead || isAttackTrigger) return;

        currentTimer += Time.deltaTime;

        if (currentTimer > triggerDuration)
        {
            isAttackTrigger = true;
            EnemyActionEvents.CloseAttackEvent();
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
