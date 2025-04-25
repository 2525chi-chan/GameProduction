using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�쐬�ҁF�K��

public class Dodge : MonoBehaviour
{
    [Header("1�񂲂Ƃ̉������")]
    [SerializeField] float dodgeDuration = 1f;
    [Header("�����ԉ�����A���ɂł���悤�ɂȂ�܂ł̎���")]
    [SerializeField] float dodgeInterval;
    [Header("����ɂ��ړ����x�̉�����")]
    [SerializeField] float dodgeSpeed = 2.0f;

    float dodgeTimer = 0f; //�����ԂɈڍs���Ă���̌o�ߎ��Ԍv���p
    float intervalTimer = 0f; //�����ԉ�����̌o�ߎ��Ԍv���p
    bool isDodging = false; //��𒆂��ǂ���

    public bool IsDodging {  get { return isDodging; } }

    void Update()
    {
        if (isDodging)
        {
            dodgeTimer += Time.deltaTime;

            if (dodgeTimer >= dodgeDuration)
            {
                isDodging = false;
                dodgeTimer = 0f;
                intervalTimer = 0f;
            }
        }

        else
        {
            if (intervalTimer < dodgeInterval)
                intervalTimer += Time.deltaTime;
        }
    }

    public void TryDodge() //�������
    {
        if (isDodging || intervalTimer < dodgeInterval) return;

        isDodging = true; //���
        dodgeTimer = 0f;
    }

    public float AddDodgeSpeed() //��𒆂̈ړ����x�㏸�ʂ̎擾
    {
        return isDodging ? dodgeSpeed : 0f;
    }
}
