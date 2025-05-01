//�쐬�ҁ@����
//�ohttps://bluebirdofoz.hatenablog.com/entry/2023/03/28/224836�p���Q�l�ɍ쐬

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    /// �Œ莲�̒�`
    private enum LockAxis
    {
        None,
        X,
        Y,
    }

    [Header("�Œ肷���]���̎w��")]
    [SerializeField] private LockAxis lockAxis;

    [Header("UI�̏ꍇ��Z���������]���邽�߁A�`�F�b�N�{�b�N�X�Ƀ`�F�b�N�����Ă�������")]
    [SerializeField] private bool reverseFront;

    Transform mainCamera;   //��ɐ��ʂɂ�����J�����̍��W

    private void Start()
    {
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Transform>();    //Scene������MainCamera���Q��
    }

    private void Update()
    {
        // ���I�u�W�F�N�g���烁�C���J���������̃x�N�g�����擾����
        Vector3 direction = mainCamera.transform.position - this.transform.position;

        // �x�N�g���̌Œ莲���l������
        Vector3 lockDirection = lockAxis switch
        {
            // ���b�N���Ȃ��̏ꍇ�̓x�N�g�������̂܂ܗ��p����
            LockAxis.None => direction,
            // X���Œ�̏ꍇ��X�������̃x�N�g���̕ϗʂ�0�ɂ���
            LockAxis.X => new Vector3(0.0f, direction.y, direction.z),
            // Y���Œ�̏ꍇ��Y�������̃x�N�g���̕ϗʂ�0�ɂ���
            LockAxis.Y => new Vector3(direction.x, 0.0f, direction.z),
            _ => throw new ArgumentOutOfRangeException()
        };

        // �I�u�W�F�N�g���x�N�g�������ɏ]���ĉ�]������
        // (���ʕ������t�]����ꍇ�̓x�N�g���Ƀ}�C�i�X��������)
        transform.rotation = Quaternion.LookRotation(reverseFront ? -lockDirection : lockDirection);
    }
}
