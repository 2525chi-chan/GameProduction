using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�쐬�ҁF�K��

public class DamageToTarget : MonoBehaviour
{
    float damage;
    float forwardKnockbackForce;
    float upwardKnockbackForce;

    public float Damage { get { return damage; } set { damage = value; } }
    public float ForwardKnockbackForce { get { return forwardKnockbackForce; } set { forwardKnockbackForce = value; } }
    public float UpwardKnockbackForce { get { return upwardKnockbackForce; } set { upwardKnockbackForce = value; } }

    public void AddDamageToPlayer(GameObject player) //�_���[�W��^����
    {
        PlayerStatus playerStatus = player.GetComponent<PlayerStatus>();

        // �Ȃ���Ύq�I�u�W�F�N�g����T��
        if (playerStatus == null)  playerStatus = player.GetComponentInChildren<PlayerStatus>();

        if (playerStatus == null)
        {
            Debug.LogWarning($"PlayerStatus �� {player.name} ����сA���̎q�Ɍ�����܂���ł���");
            return;
        }

        playerStatus.Hp -= damage;
        Debug.Log(damage + "�_���[�W��^����");
    }

    public void AddDamageToEnemy(GameObject enemy)
    {
        EnemyStatus enemyStatus = enemy.GetComponent<EnemyStatus>();

        // �Ȃ���Ύq���I�u�W�F�N�g����T��
        if (enemyStatus == null)
        {
            enemyStatus = enemy.GetComponentInChildren<EnemyStatus>();
        }

        if (enemyStatus == null)
        {
            Debug.LogWarning($"EnemyStatus �� {enemy.name} ����сA���̎q�Ɍ�����܂���ł���");
            return;
        }

        enemyStatus.Hp -= damage;
        Debug.Log(damage + "�_���[�W��^����");
    }

    public void ApplyKnockback(GameObject enemy) //������ԗ͂�������
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
