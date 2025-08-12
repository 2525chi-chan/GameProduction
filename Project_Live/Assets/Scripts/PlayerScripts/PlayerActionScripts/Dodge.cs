using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�쐬�ҁF�K��

public class Dodge : MonoBehaviour
{
    [Header("���������Ώ�")]
    [SerializeField] Transform target;
    [Header("1�񂲂Ƃ̉������")]
    [SerializeField] float dodgeDuration = 0.5f;
    [Header("�����ԉ�����A���ɂł���悤�ɂȂ�܂ł̎���")]
    [SerializeField] float dodgeInterval = 0.5f;
    [Header("����ɂ��ړ����x�̉�����")]
    [SerializeField] float dodgeSpeed = 20f;
    [Header("��𒆂ł��邱�Ƃ������G�t�F�N�g")]
    [SerializeField] ParticleSystem dodgeEffect;

    [Header("�K�v�ȃR���|�[�l���g")]
    [SerializeField] PlayerStatus status;

    float dodgeTimer = 0f; //�����ԂɈڍs���Ă���̌o�ߎ��Ԍv���p
    float intervalTimer = 0f; //�����ԉ�����̌o�ߎ��Ԍv���p
    bool isDodging = false; //��𒆂��ǂ���

    public float DodgeInterval { get { return dodgeInterval; } }
    public float IntervalTimer { get { return intervalTimer; } }
    public bool IsDodging { get { return isDodging; } }

    void Start()
    {
        dodgeTimer = dodgeInterval; //����̂݁A���Ԃ�҂����ɉ���ł���悤�ɂ���

        //Debug.Log(dodgeTimer);
    }
    void Update()
    {
        if (isDodging) return;

        if (intervalTimer < dodgeInterval) intervalTimer += Time.deltaTime;
    }

    public void TryDodge() //����̎n������
    {
        if (isDodging) return;

        isDodging = true; //���
        status.CurrentState = PlayerStatus.PlayerState.Invincible; //�v���C���[�𖳓G��Ԃɂ���
        dodgeEffect.Play();
        dodgeTimer = 0f;
    }

    public void DodgeProcess() //�������
    {
        if (isDodging)
        {
            DodgeMove();

            dodgeTimer += Time.deltaTime;

            if (dodgeTimer >= dodgeDuration)
            {
                isDodging = false;
                dodgeTimer = 0f;
                intervalTimer = 0f;
                status.CurrentState = PlayerStatus.PlayerState.Normal; //�v���C���[�̖��G��Ԃ���������
                dodgeEffect.Stop();
                PlayerActionEvents.IdleEvent(); //�ҋ@��ԂɈڍs����
            }
        }
    }

    void DodgeMove() //��𒆂̈ړ�����
    {
        if (target == null || !isDodging) return; //�ړ�������Ώۂ��ݒ肳��Ă��Ȃ����A��𒆂łȂ��ꍇ�͉����s��Ȃ�

        target.position += target.forward  * dodgeSpeed * Time.deltaTime;
    }
}
