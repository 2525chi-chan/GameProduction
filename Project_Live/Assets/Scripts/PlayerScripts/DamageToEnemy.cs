using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�쐬�ҁF�K��

public class DamageToEnemy : MonoBehaviour
{
    float damage;

    public float Damage { get { return damage; } set { damage = value; } }

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
}
