using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    [Header("�U�����n�߂锻����s���̈�")]
    [SerializeField] SphereCollider triggerRange;
    [Header("������s���Ώۂ̃I�u�W�F�N�g")]
    [SerializeField] string targetTag = "Player";
    
    [Header("�U�����s���܂łɕK�v�Ȕ��莞��")]
    [SerializeField] float triggerDuration = 0.8f;

    [Header("�K�v�ȃR���|�[�l���g")]
    [SerializeField] EnemyStatus enemyStatus;

    float currentTimer = 0f; //�o�ߎ��Ԃ̑���p
    bool isAttackTrigger = false; //�U�����邩�ǂ���

    public bool IsAttackTrigger { get { return isAttackTrigger; } set { isAttackTrigger = value; } }

    void OnTriggerStay(Collider other) //�U��������s���G���A�Ƀv���C���[���N�����Ă���Ƃ��̏���
    {
        if (!other.CompareTag(targetTag)) return; //�����蔻����s���I�u�W�F�N�g�̃^�O��Player�łȂ���Ώ������s��Ȃ�

        if (enemyStatus.IsDead || isAttackTrigger == true) return; //�G��HP��0�A�܂��͍U���̏������J�n���Ă���ꍇ�͉������Ȃ�

        currentTimer += Time.deltaTime;

        if (currentTimer > triggerDuration)
        {
            isAttackTrigger = true;//�U���̏���
        }
    }

    void OnTriggerExit(Collider other)�@//�U��������s���G���A����v���C���[���o���Ƃ��̏���
    {
        currentTimer = 0f;
    }
}
