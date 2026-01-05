using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

//�쐬�ҁF�K��

[System.Serializable]
class ComboStep
{
    [Header("攻撃判定")]
    [SerializeField] public GameObject hitbox;
    [Header("攻撃命中時に発生させるエフェクト")]
    [SerializeField] public GameObject hitEffect;
    [Header("攻撃時に発生させるエフェクト")]
    [SerializeField]public GameObject attackEffect;
    [Header("エフェクトの発生位置")]
    [SerializeField]public Transform attackEffectPos;
    [Header("基礎ダメージ")]
    [SerializeField] public float baseDamage = 10f;
    [Header("後ろに吹き飛ばす力")]
    [SerializeField] public float baceForwardKnockbackForce = 1f;
    [Header("真上に吹き飛ばす力")]
    [SerializeField] public float baceUpwardKnockbackForce = 1f;
    [Header("引き寄せるかどうか")]
    [SerializeField] public bool enableSuction = false;
    [Header("引き寄せる力")]
    [SerializeField] public float suctionForce = 10f;
    [Header("判定の継続時間")]
    [SerializeField] public float attackDuration = 0.2f;
    [Header("命中時のSE")]
    [SerializeField]public  AudioClip hitSound;
    [Header("次の攻撃の猶予時間")]
    [SerializeField] public float comboResetTime = 1f;
    [Header("入力受付から攻撃の発生時間")]
    [SerializeField] public float windupTime = 0.2f;
    [Header("攻撃時に移動する距離")]
    [SerializeField] public float attackMoveDistance = 1f;
   

}

public class CloseAttack : MonoBehaviour
{
    [Header("�ړ��𐧌䂷��I�u�W�F�N�g")]
    [SerializeField] Transform target;
    [Header("�R���{�ݒ�")]
    [SerializeField] List<ComboStep> comboSteps = new List<ComboStep>();
    [Header("�U�����L���ɂ���g���C�������_���[")]
    [SerializeField] public List<TrailRenderer> renderers = new List<TrailRenderer>();
    [Header("�K�v�ȃR���|�[�l���g")]
    [SerializeField] PlayerStatus playerStatus;
    [SerializeField] DamageToTarget damageToTarget;
    [SerializeField] MovePlayer movePlayer;
    [SerializeField] Live2DController live2DController;
    [SerializeField]Live2DTalkPlayer live2DTalkPlayer;
    public enum AttackState { None, Windup, Attacking, Recovering }

    AttackState attackState = AttackState.None;

    int currentComboIndex = 0; //���݂̃R���{�i�K�������ϐ�
    float lastAttackTime = 0f; //�Ō�ɍU����������
    bool isAttackBuffered = false; //�U�����͂����������ǂ���
    float stateTimer = 0f; //�e��Ԃ̌o�ߎ��Ԃ̌v���p

    float movedDistance = 0f;
    float totalMoveDistance = 0f;

    public AttackState CurrentAttackState { get { return attackState; } private set { attackState = value; } }
    public int CurrentComboIndex { get { return currentComboIndex; } }

    public void TryAttack() //�U�������i�ߐڍU���{�^�����������Ƃ��ɌĂ΂��j
    {
        if (isAttackBuffered || currentComboIndex >= comboSteps.Count) return;

        isAttackBuffered = true;
        attackState = AttackState.Windup;

        ComboStep step = comboSteps[currentComboIndex];
        totalMoveDistance = step.attackMoveDistance;
        movedDistance = 0f;
        
        //Debug.Log(currentComboIndex + 1 + "�i��");
    }

    public void CloseAttackProcess()
    {
        stateTimer += Time.deltaTime;

        HandleAttackMovement();

        switch (attackState)
        {
            case AttackState.Windup: //�U���ҋ@
                movePlayer.MoveSpeedMultiplier = 0f; //�ړ��𐧌�
                if (stateTimer >= comboSteps[currentComboIndex].windupTime)
                    BeginAttack();
                break;

            case AttackState.Attacking: //�U����
                if (stateTimer >= comboSteps[currentComboIndex].attackDuration)
                    EndAttack();
                
                  
                break;

            case AttackState.Recovering: //�U����
                if (Time.time - lastAttackTime > GetCurrentComboResetTime())
                    ResetCombo();
                break;

            case AttackState.None:
                break;
        }
    }

    void BeginAttack() //�U���J�n���̏���
    {
        ComboStep step = comboSteps[currentComboIndex];

        damageToTarget.Damage = GetCurrentDamage(); //�^����_���[�W�̑��
        damageToTarget.ForwardKnockbackForce = GetCurrentForwardForce(); //�O�����֐�����΂��͂̑��
        damageToTarget.UpwardKnockbackForce = GetCurrentUpwardForce(); //������֐�����΂��͂̑��
        damageToTarget.HitEffect = comboSteps[currentComboIndex].hitEffect; //�J�n���ꂽ�U���̖������G�t�F�N�g�̐ݒ�
        damageToTarget.HitSound = comboSteps[currentComboIndex].hitSound; //�J�n���ꂽ�U���̖������̉��ݒ�
        damageToTarget.EnableSuction = comboSteps[currentComboIndex].enableSuction; //�����񂹂�͂��L�����ǂ����̐ݒ�
        damageToTarget.SuctionForce = comboSteps[currentComboIndex].suctionForce; //�����񂹂�͂̑��

        if (step.hitbox != null) step.hitbox.SetActive(true); //�U������̗L����

        movePlayer.RotationSpeedMultiplier = 0f; //�v���C���[�̉�]�X�s�[�h�̐���
        
        foreach(var trail in renderers)
        {
            trail.enabled = true;
            
        }
        stateTimer = 0f;
        attackState = AttackState.Attacking;
        if (step.attackEffect != null)
        {
          GameObject effect  =Instantiate(step.attackEffect,step.attackEffectPos);
            effect.transform.SetParent(null);
        }
        
        Live2DPlay();

        //Debug.Log(currentComboIndex + 1 + "�i�ڔ���");
    }

    public void Live2DPlay()//Live2D�̍U�����[�V�����ƃZ���t�Đ�
    {
        if(live2DController == null || live2DTalkPlayer == null) return;

        live2DTalkPlayer.PlayTalk("Attack_" + (currentComboIndex + 1).ToString());
        //Debug.Log("Attack_" + currentComboIndex + 1);
        if (currentComboIndex == comboSteps.Count - 1)//�ŏI�i�̏ꍇ
        {
            live2DController.PlayMotion("Attack_High");
        }
        else
        {
            live2DController.PlayMotion("Attack_Low");
        }
    }
    void EndAttack() //���������U���̏I������
    {
        ComboStep step = comboSteps[currentComboIndex];

        if (step.hitbox != null) step.hitbox.SetActive(false); //�U������̖�����

        movePlayer.RotationSpeedMultiplier = 1f;

        lastAttackTime = Time.time;      
        isAttackBuffered = false;
        stateTimer = 0f;
        attackState = AttackState.Recovering;

    //    foreach (var trail in renderers) { trail.enabled = false; }
        //Debug.Log(currentComboIndex + 1 + "�i�ڏI��");
        currentComboIndex++;
    }

    void ResetCombo() //�R���{�i�K�̏�����
    {
        //�e�����蔻��̖�����
        foreach (var step in comboSteps)
        {
            if (step.hitbox != null) step.hitbox.SetActive(false);
            foreach (var trail in renderers) { 
            trail.enabled = false;
           
            }        
        
        }
           
      
        movePlayer.MoveSpeedMultiplier = 1f;
        isAttackBuffered = false;
        stateTimer = 0f;
        attackState = AttackState.None;        

        //Debug.Log(currentComboIndex + "�R���{�̃��Z�b�g");
        currentComboIndex = 0;
        PlayerActionEvents.IdleEvent();
    }

    float GetCurrentComboResetTime() //���̃R���{�i�K�܂ł̗P�\���Ԃ̎擾
    {
        return comboSteps[currentComboIndex - 1].comboResetTime;
    }

    void HandleAttackMovement() //�U�����̑O�i����
    {
        if (attackState != AttackState.Windup || currentComboIndex >= comboSteps.Count) return;

        ComboStep step = comboSteps[currentComboIndex];

        float duration = step.windupTime; //windupTime�̊ԂɑO�i���I����
        float movePerSecond = totalMoveDistance / duration;
        float moveDelta = movePerSecond * Time.deltaTime;

        float remaining = totalMoveDistance - movedDistance;
        float actualMove = Mathf.Min(moveDelta, remaining);

        target.position += target.forward.normalized * actualMove;
        movedDistance += actualMove;
    }

    float GetCurrentDamage() //���݂̒i�̃_���[�W�ʂ��擾����
    {
        if (currentComboIndex >= comboSteps.Count || currentComboIndex < 0) return 0f;

        ComboStep step = comboSteps[currentComboIndex];
        float attackPower = playerStatus != null ? playerStatus.AttackPower : 1f;

        return step.baseDamage * attackPower; //�ŏI�I�ȃ_���[�W�ʂ�Ԃ�
    }

    float GetCurrentForwardForce() //���݂̍U���̑O�����ւ̐�����΂��͂��擾����
    {
        if (currentComboIndex >= comboSteps.Count || currentComboIndex < 0) return 0f;

        ComboStep step = comboSteps[currentComboIndex];
        float attackPower = playerStatus != null ? playerStatus.AttackPower : 1f;

        return step.baceForwardKnockbackForce * attackPower; //�ŏI�I�ȑO�����ւ̐�����΂��͂�Ԃ�
    }

    float GetCurrentUpwardForce() //���݂̍U���̏�����ւ̐�����΂��͂��擾����
    {
        if (currentComboIndex >= comboSteps.Count || currentComboIndex < 0) return 0f;

        ComboStep step = comboSteps[currentComboIndex];
        float attackPower = playerStatus != null ? playerStatus.AttackPower : 1f;

        return step.baceUpwardKnockbackForce * attackPower; //�ŏI�I�ȑO�����ւ̐�����΂��͂�Ԃ�
    }
}
