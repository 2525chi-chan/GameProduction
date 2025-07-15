using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    [Header("�U����������I�u�W�F�N�g")]
    [SerializeField] GameObject attackPrefab;
    [Header("�U��������܂ł̎���")]
    [SerializeField] float attackDuration = 1.0f;
    [Header("�U���𐶐�����ʒu")]
    [SerializeField] Transform attackPos;
    [Header("�K�v�ȃR���|�[�l���g")]
    [SerializeField] AttackTrigger attackTrigger;

    float attackTimer = 0f; //�U���ҋ@���Ԃ̌v���p�ϐ�

    void Update()
    {
        if (!attackTrigger.IsAttackTrigger) return;

        attackTimer += Time.deltaTime;

        if (attackTimer > attackDuration) InstanceAttack(); //��莞�Ԍo�ߌ�ɍU������
    }

    void InstanceAttack() //�U������
    {
        Instantiate(attackPrefab, attackPos.position, attackPos.rotation);

        attackTimer = 0f;
        attackTrigger.IsAttackTrigger = false;
    }
}
