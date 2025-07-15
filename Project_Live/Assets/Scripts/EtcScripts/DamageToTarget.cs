using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//作成者：桑原

public class DamageToTarget : MonoBehaviour
{
    float damage;
    float forwardKnockbackForce;
    float upwardKnockbackForce;

    public float Damage { get { return damage; } set { damage = value; } }
    public float ForwardKnockbackForce { get { return forwardKnockbackForce; } set { forwardKnockbackForce = value; } }
    public float UpwardKnockbackForce { get { return upwardKnockbackForce; } set { upwardKnockbackForce = value; } }

    public void AddDamageToPlayer(GameObject player) //ダメージを与える
    {
        PlayerStatus playerStatus = player.GetComponent<PlayerStatus>();

        // なければ子オブジェクトから探す
        if (playerStatus == null)  playerStatus = player.GetComponentInChildren<PlayerStatus>();

        if (playerStatus == null)
        {
            Debug.LogWarning($"PlayerStatus が {player.name} および、その子に見つかりませんでした");
            return;
        }

        playerStatus.Hp -= damage;
        Debug.Log(damage + "ダメージを与えた");
    }

    public void AddDamageToEnemy(GameObject enemy)
    {
        EnemyStatus enemyStatus = enemy.GetComponent<EnemyStatus>();

        // なければ子孫オブジェクトから探す
        if (enemyStatus == null)
        {
            enemyStatus = enemy.GetComponentInChildren<EnemyStatus>();
        }

        if (enemyStatus == null)
        {
            Debug.LogWarning($"EnemyStatus が {enemy.name} および、その子に見つかりませんでした");
            return;
        }

        enemyStatus.Hp -= damage;
        Debug.Log(damage + "ダメージを与えた");
    }

    public void ApplyKnockback(GameObject enemy) //吹き飛ぶ力を加える
    {
        Rigidbody rb = enemy.GetComponent<Rigidbody>();

        if (rb != null && !rb.isKinematic)
        {
            Vector3 forwardDirection = (enemy.transform.position - transform.position).normalized;

            Vector3 knockback = forwardDirection * forwardKnockbackForce + Vector3.up * upwardKnockbackForce;

            rb.AddForce(knockback, ForceMode.Impulse);
        }
    }
}
