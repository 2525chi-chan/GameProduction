using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    [Header("攻撃判定を持つオブジェクト")]
    [SerializeField] GameObject attackPrefab;
    [Header("攻撃をするまでの時間")]
    [SerializeField] float attackDuration = 0.7f;
    [Header("攻撃後に発生するクールタイム")]
    [SerializeField] float attackCoolTime = 0.2f;
    [Header("攻撃を生成する位置")]
    [SerializeField] Transform attackPos;
    [Header("必要なコンポーネント")]
    [SerializeField] AttackTrigger attackTrigger;
    [SerializeField] EnemyMover mover;
   
    public float AttackDuration {  get { return attackDuration; } }
    public float AttackCoolTime { get { return attackCoolTime; } }

    public void InstanceAttack() //攻撃処理
    {
        if (!attackTrigger.IsAttackTrigger) return;

        GameObject attackObj = Instantiate(attackPrefab, attackPos.position, attackPos.rotation);
        
        if (mover != null)
        {
            var hitbox = attackObj.GetComponent<HitboxTrigger>();

            if (hitbox != null)
                hitbox.SetOwnerMoveType(mover.MoveType); //攻撃判定に、攻撃者の移動タイプを渡す
        }
    }
}
