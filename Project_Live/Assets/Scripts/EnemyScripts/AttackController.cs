using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    [Header("攻撃判定を持つオブジェクト")]
    [SerializeField] GameObject attackPrefab;
    [Header("攻撃をするまでの時間")]
    [SerializeField] float attackDuration = 1.0f;
    [Header("攻撃を生成する位置")]
    [SerializeField] Transform attackPos;
    [Header("必要なコンポーネント")]
    [SerializeField] AttackTrigger attackTrigger;
    [SerializeField] EnemyMover mover;

    public float AttackDuration { get { return attackDuration; } }

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

        var attackParameters = attackObj.GetComponent<AttackParameter>();
        if (attackParameters != null)
            attackParameters.SetOwner(transform.parent.gameObject); //生成した攻撃に、攻撃者の情報を渡す（この場合は、攻撃判定を持つ敵の情報を渡している）
    }
}
