using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;  


   public  enum EnemyMoveState { stop, lookOnly, move }
public class EnemyMover : MonoBehaviour
{
  public enum EnemyMoveType { PlayerChase, BlockPlayer, StageDestroy }


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
    GameObject[] breakables; //�j��ł���Ώ�
    EnemyMoveType moveType;
    EnemyMoveState moveState;

  public EnemyMoveState MoveState
    {
        get { return moveState; }   
    }

   

    public void SetMoveType(EnemyMoveType moveType)
    {
        this.moveType = moveType;
        InitTarget(); //�ړ��^�C�v�ɉ����ă^�[�Q�b�g��ݒ肷��
    }
    public void MoveSetState(EnemyMoveState state)
    {
        moveState = state;
    }
    void Start()
    {
        //moveState = MoveState.stop;
        MoveSetState(EnemyMoveState.stop);
    }

    void Update()
    {
        if (enemyStatus.IsDead) return; //HP��0�A�܂��͒ǂ�������Ώۂ�������Ȃ��ꍇ

        if(enemyStatus.IsRagdoll) return; //���O�h�[����Ԃ̂Ƃ��͈ړ����Ȃ�

        if (moveType == EnemyMoveType.StageDestroy && lookTarget == null)
        {
            InitTarget();
            Debug.Log("�^�[�Q�b�g�̍Đݒ�");
        }

        if (lookTarget != null)
        {
            float distance = Vector3.Distance(transform.position, lookTarget.position); //�v���C���[�Ƃ̋������Z�o����

            MoveTypeProcess(distance);
            MoveStateProcess();
        }
    }

    void InitTarget() //�ړ��^�C�v�ɉ����ă^�[�Q�b�g��ݒ肷��
    {
        switch (moveType)
        {
            case EnemyMoveType.PlayerChase: //�v���C���[�̈ʒu�Ɋւ�炸�A�v���C���[�Ɍ������Ĉړ�����

            case EnemyMoveType.BlockPlayer: //�v���C���[����苗���܂ŋ߂Â����Ƃ��̂݃v���C���[�Ɍ������Ĉړ�����
                GameObject player = GameObject.FindGameObjectWithTag("LookPoint");
                if (player != null) lookTarget = player.transform;
                break;

            case EnemyMoveType.StageDestroy:
                breakables = GameObject.FindGameObjectsWithTag("Breakable"); //�U���ł���I�u�W�F�N�g�ɐݒ肳��Ă���^�O����()���ɋL�q����
                Debug.Log(breakables.Length);
                if (breakables.Length > 0)
                {
                    lookTarget = GetNearestTarget(breakables); //��ԋ߂��I�u�W�F�N�g�Ɍ������Ĉړ�����
                }

                else return;
                break;
        }
    }

    void MoveTypeProcess(float distance) //�ړ��^�C�v�ɉ������A�ړ���Ԃ̑J�ڂ��s��
    {
        switch (moveType)
        {
            case EnemyMoveType.PlayerChase:
                if (distance >= stopRange) moveState = EnemyMoveState.move;
                else moveState = EnemyMoveState.stop;
                break;

            case EnemyMoveType.BlockPlayer:
                if (distance <= detectionRange && distance >= stopRange) moveState = EnemyMoveState.move;
                else if (distance < stopRange) moveState = EnemyMoveState.lookOnly;
                else moveState = EnemyMoveState.stop;
                break;

            case EnemyMoveType.StageDestroy:
                if (lookTarget == null || !lookTarget.gameObject.activeInHierarchy)
                {
                    breakables = GameObject.FindGameObjectsWithTag("Breakable");
                    lookTarget = (breakables.Length > 0) ? GetNearestTarget(breakables) : null;
                }

                moveState = (lookTarget != null && distance >= stopRange) ? EnemyMoveState.move : EnemyMoveState.stop;
                break;
        }
    }

    void MoveStateProcess() //�ړ���Ԃ��Ƃ̈ړ��������s��
    {
        switch (moveState)
        {
            case   EnemyMoveState.stop: //��~��ԁi�v���C���[��Ǐ]����K�v���Ȃ��j
                return;

            case EnemyMoveState.lookOnly: //�v���C���[�̕��������������̂ݍs�����
                LookPlayer();
                return;

            case EnemyMoveState.move: //�v���C���[�̕����������ĒǏ]������
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

    Transform GetNearestTarget(GameObject[] objects)
    {
        Transform nearest = null;

        float minDist = Mathf.Infinity;

        foreach (GameObject obj in objects)
        {
            float dist = Vector3.Distance(transform.position, obj.transform.position);

            if (dist < minDist)
            {
                minDist = dist;
                nearest = obj.transform;
            }
        }

        return nearest;
    }

    
}
