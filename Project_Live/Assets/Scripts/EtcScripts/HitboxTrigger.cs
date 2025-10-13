using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�쐬�ҁF�K��

public class HitboxTrigger : MonoBehaviour
{
    [Header("�����𔻒肷���")]
    [SerializeField] int MaxHitCount = 1;
    [Header("����������s�����Ԃ̊Ԋu")]
    [SerializeField] float hitIntervalTime = 0.5f;

    [Header("�K�v�ȃR���|�[�l���g")]
    [SerializeField] DamageToTarget damageToTarget;

    Dictionary<Collider, float> hitIntervalTimers = new Dictionary<Collider, float>(); //�G���Ƃ̍Ō�ɍU�����������Ă���̌o�ߎ���
    Dictionary<Collider, int> hitCounts = new Dictionary<Collider, int>(); //�G���Ƃ̍��܂ōU��������������

    List<string> ignoreTags = new List<string>();
    EnemyMover.EnemyMoveType ownermoveType;

    bool isOwnerSet = false; //�v���C���[�̍U�����ǂ����̔���p

    void Update() //�o�ߎ��Ԃ̍X�V
    {
        var keys = new List<Collider>(hitIntervalTimers.Keys);

        foreach (var col in keys)
            hitIntervalTimers[col] += Time.deltaTime;
    }

    void OnTriggerStay(Collider other)
    {
        SetIgnoreTags();

        if (ignoreTags.Contains(other.tag)) return; //�o�^����Ă���^�O�Ȃ牽�����Ȃ�

        if (!hitCounts.ContainsKey(other)) //���߂čU�������������G�Ȃ�
        {
            hitCounts[other] = 0; //�����񐔂̃��Z�b�g
            hitIntervalTimers[other] = hitIntervalTime; //�o�ߎ��Ԃ̃��Z�b�g
        }

        if (hitCounts[other] >= MaxHitCount) return; //�����񐔂𐧌�����

        if (hitIntervalTimers[other] >= hitIntervalTime) //�q�b�g�\�Ȏ��Ԃ��o�߂��Ă����ꍇ
        {
            ApplyDamage(other);
            
            damageToTarget?.ApplyKnockback(other); //������я���

            hitIntervalTimers[other] = 0f; //�U��������̌o�ߎ��Ԃ����Z�b�g����
            hitCounts[other]++; //���݂̃q�b�g�񐔂𑝂₷
            //Debug.Log(hitCounts[other]);
        }
    }

    void OnDisable()
    {
        ResetHits();
    }

    public void ResetHits() //�����񐔂̃��Z�b�g
    {
        hitCounts.Clear();
        hitIntervalTimers.Clear();
    }

    public void SetOwnerMoveType(EnemyMover.EnemyMoveType moveType) //�U���𐶐�������̈ړ��^�C�v��n��
    {
        ownermoveType = moveType;
        isOwnerSet = true;
    }

    void SetIgnoreTags() //����������s���Ώۂ̐ݒ�
    {
        ignoreTags.Clear();

        //�G�̈ړ��^�C�v���n����Ă��Ȃ��ꍇ�A�v���C���[�̍U���Ƃ���
        if (!isOwnerSet)
        {
            ignoreTags.Add("Player");
            return;
        }

        switch (ownermoveType)
        {
            //�v���C���[��ǂ��G �� �����i�G���m�j�͖�������
            case EnemyMover.EnemyMoveType.PlayerChase:
            case EnemyMover.EnemyMoveType.BlockPlayer:                
                ignoreTags.Add("Enemy");
                ignoreTags.Add("Breakable");
                break;

            //�X�e�[�W�j��^ �� �����i�G���m�j�A�v���C���[�𖳎�����
            case EnemyMover.EnemyMoveType.StageDestroy:
                ignoreTags.Add("Player");
                ignoreTags.Add("Enemy");
                break;

            default: break;
        }

        //foreach (var tag in ignoreTags)
        //    Debug.Log(tag);
    }

    void ApplyDamage(Collider other) //�_���[�W����
    {
        if (other.CompareTag("Player")) damageToTarget?.AddDamageToPlayer(other); //�v���C���[�ւ̃_���[�W����
        else if (other.CompareTag("Enemy")) damageToTarget?.AddDamageToEnemy(other); //�G�ւ̃_���[�W����
        else if (other.CompareTag("Breakable")) damageToTarget?.AddDamageToObject(other); //�j��\�I�u�W�F�N�g�ւ̃_���[�W�����i���j
    }
}
