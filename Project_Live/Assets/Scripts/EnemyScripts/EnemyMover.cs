using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    enum MoveState { stop, lookOnly, move }

    [Header("�ړ����邩���f����v���C���[�Ƃ̋���")]
    [SerializeField] float detectionRange = 10f;
    [Header("��~���邩���f����v���C���[�Ƃ̋���")]
    [SerializeField] float stopRange = 1f;
    [Header("�ړ����x")]
    [SerializeField] float moveSpeed = 3f;
    [Header("��]���x")]
    [SerializeField] float rotateSpeed;

    [Header("�K�v�ȃR���|�[�l���g")]
    [SerializeField] EnemyStatus enemyStatus;

    private Transform lookTarget; //�ǂ�������Ώ�

    MoveState moveState;

    void Start()
    {
        moveState = MoveState.stop;
        lookTarget = GameObject.FindGameObjectWithTag("LookPoint").transform;
    }

    void Update()
    {
        if (enemyStatus.IsDead || lookTarget == null) return; //HP��0�A�܂��͒ǂ�������Ώۂ�������Ȃ��ꍇ

        float distance = Vector3.Distance(transform.position, lookTarget.position); //�v���C���[�Ƃ̋������Z�o����

        if (distance <= detectionRange && distance >= stopRange) //���ȏ㋗�������ꂽ��
            moveState = MoveState.move;

        else if (distance <= detectionRange && distance < stopRange) //��苗���ȉ��܂ŋ߂Â�����
            moveState = MoveState.lookOnly;

        else moveState = MoveState.stop; //���ȏ㋗�������ꂽ��

        switch(moveState)
        {
            case MoveState.stop: //��~��ԁi�v���C���[��Ǐ]����K�v���Ȃ��j
                return;

            case MoveState.lookOnly: //�v���C���[�̕��������������̂ݍs�����
                LookPlayer();
                return;

            case MoveState.move: //�v���C���[�̕����������ĒǏ]������
                LookPlayer();
                MoveTowardsPlayer();
                return;

            default:
                return;
        }
    }

    void MoveTowardsPlayer()//�v���C���[�Ɍ������Ĉړ�����
    {
        Vector3 direction = (lookTarget.position - transform.position).normalized;

        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    void LookPlayer()//Y�������ς���
    {
        Vector3 playerPos = lookTarget.position;
        playerPos.y = lookTarget.transform.position.y;

        Vector3 direction = (playerPos - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        targetRotation.x = 0f;
        targetRotation.z = 0f;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }
}
