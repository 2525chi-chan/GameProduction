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

    float attackTimer = 0f; //攻撃待機時間の計測用変数

    void Update()
    {
        if (!attackTrigger.IsAttackTrigger) return;

        attackTimer += Time.deltaTime;

        if (attackTimer > attackDuration) InstanceAttack(); //一定時間経過後に攻撃する
    }

    void InstanceAttack() //攻撃処理
    {
        Instantiate(attackPrefab, attackPos.position, attackPos.rotation);

        attackTimer = 0f;
        attackTrigger.IsAttackTrigger = false;
    }
}
