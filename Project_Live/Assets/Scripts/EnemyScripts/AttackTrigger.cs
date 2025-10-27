using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    [Header("�U�����n�߂锻����s���̈�")]
    [SerializeField] SphereCollider triggerRange;
    
    [Header("�U�����s���܂łɕK�v�Ȕ��莞��")]
    [SerializeField] float triggerDuration = 0.8f;

    [Header("�K�v�ȃR���|�[�l���g")]
    [SerializeField] EnemyStatus status;
    [SerializeField] EnemyMover mover;
    [SerializeField] Animator animator;

    float currentTimer = 0f; //�o�ߎ��Ԃ̑���p
    bool isAttackTrigger = false; //�U�����邩�ǂ���

    public bool IsAttackTrigger { get { return isAttackTrigger; } set { isAttackTrigger = value; } }

    void OnTriggerStay(Collider other) //�U��������s���G���A�Ƀv���C���[���N�����Ă���Ƃ��̏���
    {
        if (status.IsDead || isAttackTrigger == true) return; //�G��HP��0�A�܂��͍U���̏������J�n���Ă���ꍇ�͉������Ȃ�

        switch (mover.MoveType)
        {
            case EnemyMover.EnemyMoveType.PlayerChase:
            case EnemyMover.EnemyMoveType.BlockPlayer:
                if (!other.CompareTag("Player")) return;
                break;

            case EnemyMover.EnemyMoveType.StageDestroy:
                if (!other.CompareTag("Breakable")) return;
                break;

            default: break;
        }

        currentTimer += Time.deltaTime;

        if (currentTimer > triggerDuration)
        {  isAttackTrigger = true;//�U���̏���
            if(animator!=null)
            animator.SetTrigger("Attack");
        }
                                                                          
    }

    void OnTriggerExit(Collider other)�@//�U��������s���G���A����v���C���[���o���Ƃ��̏���
    {
        currentTimer = 0f;
    }

}
