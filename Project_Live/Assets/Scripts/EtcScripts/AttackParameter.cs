using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackParameter : MonoBehaviour
{
    [Header("��{�_���[�W")]
    [SerializeField] float baceDamage = 5f;
    [Header("�������ɔ���������G�t�F�N�g")]
    [SerializeField] GameObject hitEffect;
    [Header("��{�ƂȂ�O�����ւ̐�����΂���")]
    [SerializeField] float baceForwardKnockbackForce = 1f;
    [Header("��{�ƂȂ������ւ̐�����΂���")]
    [SerializeField] float baceUpwardKnockbackForce = 1f;
    [Header("��{�ƂȂ鉺�����ւ̐�����΂���")]
    [SerializeField] float baceDownwardKnockbackForce = 0f;
    //[Header("�擾����R���|�[�l���g�̃I�u�W�F�N�g��")]
    //[SerializeField] string objectName = "PlayerStatus";
    [Header("�K�v�ȃR���|�[�l���g")]
    [SerializeField] DamageToTarget damageToTarget;

    GameObject target;
    PlayerStatus status;

    void Start()
    {
        SetParameters();
    }

    void SetParameters() //�_���[�W�A�������̃G�t�F�N�g�A������΂��͂�ݒ肷��
    {
        damageToTarget.Damage = GetDamage();
        damageToTarget.HitEffect = hitEffect;
        damageToTarget.ForwardKnockbackForce = GetForwardForce();
        damageToTarget.UpwardKnockbackForce = GetUpwardForce();
        damageToTarget.DownwardKnockbackForce = GetDownwardForce();
    }

    float GetDamage() //�_���[�W�ʂ��擾����
    {
        return target != null ? baceDamage * status.AttackPower : baceDamage;
    }

    float GetForwardForce() //������ւ̐�����΂��͂��擾����
    {
        return target != null ? baceForwardKnockbackForce * status.AttackPower : baceForwardKnockbackForce;
    }

    float GetUpwardForce() //�O�����ւ̐�����΂��͂��擾����
    {
        return target != null ? baceUpwardKnockbackForce * status.AttackPower : baceUpwardKnockbackForce;
    }

    float GetDownwardForce() //�������ւ̐�����΂��͂��擾����
    {
        return target != null ? baceDownwardKnockbackForce * status.AttackPower : baceDownwardKnockbackForce;
    }
}
