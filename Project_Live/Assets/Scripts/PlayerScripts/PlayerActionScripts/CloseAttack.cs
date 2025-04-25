using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�쐬�ҁF�K��

[System.Serializable]
class ComboStep
{
    [Header("���̒i�̍U������p�I�u�W�F�N�g")]
    [SerializeField] public GameObject hitbox;
    [Header("�����蔻��̎�������")]
    [SerializeField] public float attackDuration = 0.2f;
    [Header("���̒i�Ɉڍs�\�ȗP�\���ԁi���߂���Ə��i�ɖ߂�)")]
    [SerializeField] public float comboResetTime = 1f;   
}

public class CloseAttack : MonoBehaviour
{
    [Header("�R���{�ݒ�")]
    [SerializeField] List<ComboStep> comboSteps = new List<ComboStep>();    

    int currentComboIndex = 0; //���݂̃R���{�i�K�������ϐ�
    float lastAttackTime = 0f; //�Ō�ɍU����������
    float attackTimer = 0f; //�����蔻��̎������ԗp�ϐ�
    bool isAttacking = false; //���ݍU�����Ă��邩�ǂ���

    void Update()
    {
        if (isAttacking)
        {
            attackTimer -= Time.deltaTime;

            if (attackTimer <= 0f)
                EndAttack();
        }

        if (!isAttacking && currentComboIndex != 0 && Time.time - lastAttackTime > GetCurrentComboResetTime())
            ResetCombo();
    }

    public void TryAttack() //�U������
    {
        if (isAttacking) return;

        if (currentComboIndex < comboSteps.Count)
        {
            ComboStep step = comboSteps[currentComboIndex];

            if (step.hitbox != null)
                step.hitbox.SetActive(true);

            attackTimer = step.attackDuration;
            isAttacking = true;

            currentComboIndex++;
            lastAttackTime = Time.time;
        }
    }

    void EndAttack() //���������U���̏I������
    {
        int prevIndex = currentComboIndex - 1;

        if (prevIndex >= 0 && prevIndex < comboSteps.Count)
        {
            var step = comboSteps[prevIndex];

            if (step.hitbox != null)
                step.hitbox.SetActive(false);
        }

        isAttacking = false;
    }

    void ResetCombo() //�R���{�i�K�̏�����
    {
        currentComboIndex = 0;
        isAttacking = false;
        attackTimer = 0f;

        //�e�����蔻��̖�����
        foreach (var step in comboSteps)
        {
            if (step.hitbox != null)
                step.hitbox.SetActive(false);
        }
    }

    float GetCurrentComboResetTime() //���̃R���{�i�K�܂ł̗P�\���Ԃ̎擾
    {
        return comboSteps[currentComboIndex - 1].comboResetTime;
    }
}
