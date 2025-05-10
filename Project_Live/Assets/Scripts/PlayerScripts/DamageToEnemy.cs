using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�쐬�ҁF�K��

public class DamageToEnemy : MonoBehaviour
{
    float damage;
    float knockbackForce;

    public float Damage { get { return damage; } set { damage = value; } }
    public float KnockbackForce { get { return knockbackForce; } set {  knockbackForce = value; } }

    public void TakeDamage(GameObject enemy)
    {
        EnemyStatus enemyStatus = enemy.GetComponent<EnemyStatus>();

        if (enemyStatus == null)
        {
            Debug.LogWarning("�Ώۂ�������܂���");
            return;
        }

        enemyStatus.Hp -= damage;
        //Debug.Log(damage + "�_���[�W��^����");
    }

    public void ApplyKnockback(GameObject enemy)
    {
        Rigidbody rb = enemy.GetComponent<Rigidbody>();

        if (rb != null && !rb.isKinematic)
        {
            Vector3 direction = (enemy.transform.position - transform.position).normalized;
            rb.AddForce(direction * knockbackForce, ForceMode.Impulse);
        }
    }
}
